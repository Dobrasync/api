using Dobrasync.Api.BusinessLogic.Mapper.AutoMapper;
using Dobrasync.Api.BusinessLogic.Services.Core.AppsettingsProvider;
using Dobrasync.Api.BusinessLogic.Services.Core.Localization;
using Dobrasync.Api.BusinessLogic.Services.Core.SystemSettings;
using Dobrasync.Api.BusinessLogic.Services.Main.Auth;
using Dobrasync.Api.BusinessLogic.Services.Main.File;
using Dobrasync.Api.BusinessLogic.Services.Main.Library;
using Dobrasync.Api.Database.DB;
using Dobrasync.Api.Database.Repos;
using Dobrasync.Api.Tests.Mock;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Dobrasync.Api.Tests.Common;

public static class ServiceUtil
{
    public static IServiceCollection RegisterCommonServices(IServiceCollection services)
    {
        // TODO: Maybe put this whole function into the Controllers or Shared project and have the controllers
        // call the same fnc as this?

        #region Core services

        services.AddScoped<IAppsettingsProvider, MockAppsettingsProvider>();
        services.AddLocalization();
        services.AddLogging();
        services.AddScoped<ILocalizationService, LocalizationService>();
        services.AddAutoMapper(cfg => { cfg.AddProfile<MappingProfile>(); });
        services.AddScoped<IRepoWrapper, RepoWrapper>();

        #endregion

        #region Mock

        services.AddScoped<IHttpContextAccessor, MockHttpContextAccessor>();

        #endregion

        #region DB

        services.AddDbContext<DobrasyncContext>(opt => { opt.UseInMemoryDatabase($"Test-{Guid.NewGuid()}"); });

        #endregion

        #region Services

        services.AddScoped<ILibraryService, LibraryService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ISystemSettingsService, SystemSettingsService>();
        services.AddScoped<IFileService, FileService>();

        #endregion

        return services;
    }
}