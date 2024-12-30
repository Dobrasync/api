using Dobrasync.Api.BusinessLogic.Services.Core.AppsettingsProvider;
using Dobrasync.Api.Shared.Appsettings;
using Dobrasync.Api.Shared.Appsettings.Auth;
using Dobrasync.Api.Shared.Appsettings.Auth.Idp;
using Dobrasync.Api.Shared.Appsettings.Core;
using Dobrasync.Api.Shared.Appsettings.Storage;

namespace Dobrasync.Api.Tests.Mock;

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