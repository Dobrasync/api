using Dobrasync.Api.Database.Repos;

namespace Dobrasync.Api.BusinessLogic.Services.Core.SystemSettings;

public class SystemSettingsService(IRepoWrapper repoWrap) : ISystemSettingsService
{
    public bool IsSetupComplete()
    {
        return true;
    }
}