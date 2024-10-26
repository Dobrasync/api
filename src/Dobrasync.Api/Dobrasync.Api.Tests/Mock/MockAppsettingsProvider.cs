using Lamashare.BusinessLogic.Services.Core.AppsettingsProvider;
using LamashareApi.Shared.Appsettings;
using LamashareApi.Shared.Appsettings.Auth;
using LamashareApi.Shared.Appsettings.Storage;
using LamashareApi.Shared.Appsettings.System;

namespace LamashareApi.Tests.Mock;

public class MockAppsettingsProvider : IAppsettingsProvider
{
    public Appsettings GetAppsettings()
    {
        return new Appsettings
        {
            Auth =
                new AuthAS
                {
                    Idp = new IdpAS
                    {
                        Device = new DeviceAS
                        {
                            ClientId = ""
                        },
                        Authority = string.Empty,
                        Api = new ApiAS
                        {
                            ClientId = "",
                            ClientSecret = ""
                        }
                    }
                },
            Core =
                new CoreAS
                {
                    CorsOrigins = ["test"]
                },
            Storage =
                new StorageAS
                {
                    LibraryLocation = "./serverlibs",
                    TempBlockLocation = "./tempblocks"
                }
        };
    }
}