using AutoMapper;
using Lamashare.BusinessLogic.Dtos.Auth;
using Lamashare.BusinessLogic.Services.Core.AppsettingsProvider;
using LamashareApi.Database.Repos;

namespace Lamashare.BusinessLogic.Services.Main.Auth;

public class AuthService(IRepoWrapper repoWrap, IMapper mapper, IAppsettingsProvider apps) : IAuthService
{
    public async Task<Sdto> GetIdpDeviceClientId()
    {
        return new Sdto { Content = apps.GetAppsettings().Auth.Idp.Device.ClientId };
    }

    public async Task<Sdto> GetIdpUrl()
    {
        return new Sdto { Content = apps.GetAppsettings().Auth.Idp.Authority };
    }
}