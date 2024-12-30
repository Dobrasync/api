using Dobrasync.Api.Shared.Appsettings;
using Microsoft.Extensions.Options;

namespace Dobrasync.Api.BusinessLogic.Services.Core.AppsettingsProvider;

public class AppsettingsProvider(IOptions<Appsettings> appsettingsOptions) : IAppsettingsProvider
{
    public Appsettings GetAppsettings()
    {
        return appsettingsOptions.Value;
    }
}