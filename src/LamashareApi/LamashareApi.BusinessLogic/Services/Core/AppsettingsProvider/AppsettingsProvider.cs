using LamashareApi.Shared.Appsettings;
using Microsoft.Extensions.Options;

namespace Lamashare.BusinessLogic.Services.Core.AppsettingsProvider;

public class AppsettingsProvider(IOptions<Appsettings> appsettingsOptions) : IAppsettingsProvider
{
    public Appsettings GetAppsettings()
    {
        return appsettingsOptions.Value;
    }
}