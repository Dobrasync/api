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
using LamashareApi.Shared.Appsettings;
using LamashareApi.Tests.Mock;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LamashareApi.Tests.Common;

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
        services.AddDbContext<LamashareContext>(opt =>
        {
            opt.UseInMemoryDatabase(databaseName: $"Test-{Guid.NewGuid()}");
        });
        #endregion
        #region Services
        services.AddScoped<ILibraryService, LibraryService>();
        services.AddScoped<IUsersService, UsersService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IInvokerService, InvokerService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ISystemSettingsService, SystemSettingsService>();
        services.AddScoped<IFileService, FileService>();
        #endregion

        return services;
    }
}