using AutoMapper;
using Lamashare.BusinessLogic.Dtos.Auth;
using Lamashare.BusinessLogic.Dtos.User;
using Lamashare.BusinessLogic.Services.Core.AppsettingsProvider;
using Lamashare.BusinessLogic.Services.Core.Invoker;
using LamashareApi.Database.DB.Entities;
using LamashareApi.Database.Repos;

namespace Lamashare.BusinessLogic.Services.Main.Auth;

public class AuthService(IRepoWrapper repoWrap, IMapper mapper, IAppsettingsProvider apps, IInvokerService invokerService) : IAuthService
{
    public async Task<Sdto> GetIdpDeviceClientId()
    {
        return new Sdto { Content = apps.GetAppsettings().Auth.Idp.Device.ClientId };
    }

    public async Task<Sdto> GetIdpUrl()
    {
        return new Sdto { Content = apps.GetAppsettings().Auth.Idp.Authority };
    }
    
    public async Task<SessionInfoDto> GetSessionInfo()
    {
        UserEntity invoker = await invokerService.GetInvoker();

        return new SessionInfoDto()
        {
            User = mapper.Map<UserDto>(invoker),
        };
    }
}