using System.Globalization;
using System.Text.Json.Serialization;
using Asp.Versioning;
using Lamashare.BusinessLogic.Mapper;
using Lamashare.BusinessLogic.Services.Core.AppsettingsProvider;
using Lamashare.BusinessLogic.Services.Main.Library;
using Lamashare.BusinessLogic.Services.Main.Users;
using LamashareApi.Database.DB;
using LamashareApi.Database.Repos;
using LamashareApi.Middleware.ExceptionInteceptor;
using LamashareApi.Shared.Appsettings;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

#region Appsettings

builder.Services.AddOptions<Appsettings>()
    .Bind(builder.Configuration)
    .ValidateDataAnnotations()
    .ValidateOnStart();

var appsettings = new Appsettings();
builder.Configuration.Bind(appsettings);

#endregion

#region API Explorer

builder.Services.AddEndpointsApiExplorer();

#endregion

#region Swagger

builder.Services.AddSwaggerGen(opt => { opt.EnableAnnotations(); });

#endregion

#region Localization

builder.Services.AddLocalization();
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new List<CultureInfo>
    {
        new("en-US"),
        new("de-DE")
    };

    options.DefaultRequestCulture = new RequestCulture("en-US");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
    options.ApplyCurrentCultureToResponseHeaders = true;
});

#endregion

#region Route

builder.Services.Configure<RouteOptions>(opt => { opt.LowercaseUrls = true; });

#endregion

#region Versioning

builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new ApiVersion(1, 0);
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ReportApiVersions = true;
    opt.ApiVersionReader = new UrlSegmentApiVersionReader();
}).AddApiExplorer(opt =>
{
    opt.GroupNameFormat = "'v'VVV";
    opt.SubstituteApiVersionInUrl = true;
});

#endregion

#region Controllers

builder.Services.AddControllers();

builder.Services.AddControllersWithViews()
    .AddJsonOptions(opt => opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
#endregion

#region Automapper

builder.Services.AddAutoMapper(cfg => { cfg.AddProfile<MappingProfile>(); });

#endregion

#region Services

builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IAppsettingsProvider, AppsettingsProvider>();
builder.Services.AddScoped<IRepoWrapper, RepoWrapper>();
builder.Services.AddDbContext<LamashareContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("MainDb"));
});

builder.Services.AddScoped<ILibraryService, LibraryService>();
builder.Services.AddScoped<IUsersService, UsersService>();

#endregion

#region Cors
builder.Services.AddCors();
#endregion

var app = builder.Build();

#region DevSwagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
#endregion

#region HTTPS Redirect

app.UseHttpsRedirection();

#endregion

#region Controllers
app.MapControllers();
#endregion

#region Cors

app.UseCors(opt =>
{
    opt.AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
        .WithOrigins(appsettings.Core.CorsOrigins);
});
#endregion

#region Local

var localizeOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(localizeOptions!.Value);

#endregion

#region Middleware

app.UseMiddleware<ExceptionInterceptor>();

#endregion

app.Run();