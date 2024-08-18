using Lamashare.BusinessLogic.Services.Main.System;
using LamashareApi.Database.Repos;

namespace Lamashare.BusinessLogic.Services.Main.SystemSettings;

public class SystemSettingsService(IRepoWrapper repoWrap) : ISystemSettingsService
{
    public bool IsSetupComplete()
    {
        return true;
    }
}