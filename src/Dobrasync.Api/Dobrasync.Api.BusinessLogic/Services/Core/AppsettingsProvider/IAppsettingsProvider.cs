using LamashareApi.Shared.Appsettings;

namespace Lamashare.BusinessLogic.Services.Core.AppsettingsProvider;

public interface IAppsettingsProvider
{
    public Appsettings GetAppsettings();
}