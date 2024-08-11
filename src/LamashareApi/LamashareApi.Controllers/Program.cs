using System.Globalization;
using Asp.Versioning;
using Lamashare.BusinessLogic.Mapper;
using LamashareApi.Middleware.ExceptionInteceptor;
using LamashareApi.Shared.Appsettings;
using Microsoft.AspNetCore.Localization;
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
    opt.AssumeDefaultVersionWhenUnspecified = false;
    opt.ReportApiVersions = true;
}).AddApiExplorer(opt =>
{
    opt.GroupNameFormat = "'v'VVV";
    opt.SubstituteApiVersionInUrl = false;
});

#endregion

#region Automapper

builder.Services.AddAutoMapper(cfg => { cfg.AddProfile<MappingProfile>(); });

#endregion

var app = builder.Build();

#region Localization

var localizeOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(localizeOptions!.Value);

#endregion

#region Dev swagger UI

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

#endregion

#region HTTPS Redirect

app.UseHttpsRedirection();

#endregion

#region Middleware

app.UseMiddleware<ExceptionInterceptor>();

#endregion

app.Run();