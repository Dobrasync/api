using Dobrasync.Api.BusinessLogic.Dtos.Auth;

namespace Dobrasync.Api.BusinessLogic.Services.Main.Auth;

public interface IAuthService
{
    Task<Sdto> GetIdpDeviceClientId();

    Task<Sdto> GetIdpUrl();

    Task<SessionInfoDto> GetSessionInfo();
}