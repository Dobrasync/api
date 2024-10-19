using Lamashare.BusinessLogic.Services.Core.AppsettingsProvider;
using LamashareApi.Shared.Appsettings;

namespace LamashareApi.Tests.Mock;

public class MockAppsettingsProvider : IAppsettingsProvider
{
    public Appsettings GetAppsettings()
    {
        return new()
        {
            Auth =
            new() {
                AuthJwt = new() {
                    Audience = "test",
                    Issuer = "test",
                    Secret = "test",
                    LifetimeMinutes = 3600,
                },
                Idp = new()
                {
                    Device = new()
                    {
                        ClientId = "",
                    },
                    Authority = string.Empty,
                    Web = new()
                    {
                        ClientId = ""
                    }
                }
            },
            Core =
            new() {
                CorsOrigins = ["test"]
            },
            Storage =
            new() {
                LibraryLocation = "./serverlibs",
                TempBlockLocation = "./tempblocks",
            }
        };
    }
}