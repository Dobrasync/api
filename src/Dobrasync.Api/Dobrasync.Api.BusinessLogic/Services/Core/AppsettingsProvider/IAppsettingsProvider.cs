using Dobrasync.Api.Shared.Appsettings;

namespace Dobrasync.Api.BusinessLogic.Services.Core.AppsettingsProvider;

public interface IAppsettingsProvider
{
    public Appsettings GetAppsettings();
}