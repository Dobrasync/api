using AutoMapper;
using Dobrasync.Api.BusinessLogic.Dtos.Auth;
using Dobrasync.Api.BusinessLogic.Dtos.User;
using Dobrasync.Api.BusinessLogic.Services.Core.AppsettingsProvider;
using Dobrasync.Api.BusinessLogic.Services.Core.Invoker;
using Dobrasync.Api.Database.DB.Entities;
using Dobrasync.Api.Database.Repos;

namespace Dobrasync.Api.BusinessLogic.Services.Main.Auth;

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