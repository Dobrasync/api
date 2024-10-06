using System.Globalization;
using System.Text.Json.Serialization;
using Asp.Versioning;
using Asp.Versioning.Conventions;
using Lamashare.BusinessLogic.Mapper;
using Lamashare.BusinessLogic.Mapper.AutoMapper;
using Lamashare.BusinessLogic.Services.Core.AppsettingsProvider;
using Lamashare.BusinessLogic.Services.Core.Jwt;
using Lamashare.BusinessLogic.Services.Core.Localization;
using Lamashare.BusinessLogic.Services.Main.Auth;
using Lamashare.BusinessLogic.Services.Main.File;
using Lamashare.BusinessLogic.Services.Main.InvokerService;
using Lamashare.BusinessLogic.Services.Main.Library;
using Lamashare.BusinessLogic.Services.Main.System;
using Lamashare.BusinessLogic.Services.Main.SystemSettings;
using Lamashare.BusinessLogic.Services.Main.Users;
using LamashareApi.Database.DB;
using LamashareApi.Database.Repos;
using LamashareApi.Middleware.ExceptionInteceptor;
using LamashareApi.Shared.Appsettings;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

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

builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo() { Title = "Lamashare API", Version = "v1" });
    opt.EnableAnnotations();
});

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
    opt.DefaultApiVersion = new ApiVersion(1);
    opt.ReportApiVersions = true;
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ApiVersionReader = new UrlSegmentApiVersionReader();
    opt.UnsupportedApiVersionStatusCode = 404;
}).AddMvc(opt =>
{
    opt.Conventions.Add(new VersionByNamespaceConvention());
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
builder.Services.AddScoped<ILocalizationService, LocalizationService>();
builder.Services.AddScoped<IRepoWrapper, RepoWrapper>();
builder.Services.AddDbContext<LamashareContext>(opt =>
{
    opt.UseMySQL(builder.Configuration.GetConnectionString("MainDb"));
});

builder.Services.AddScoped<ILibraryService, LibraryService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IInvokerService, InvokerService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ISystemSettingsService, SystemSettingsService>();

#endregion

#region Cors
builder.Services.AddCors();
#endregion

var app = builder.Build();

#region DevSwagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(config =>
    {
        config.SwaggerEndpoint("/swagger/v1/swagger.json", "Lamashare API");
    });
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

using(var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<LamashareContext>();
    // use context
    dbContext.Database.EnsureCreated();
}

app.Run();