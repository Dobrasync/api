using Lamashare.BusinessLogic.Dtos.Auth;

namespace Lamashare.BusinessLogic.Services.Main.Auth;

public interface IAuthService
{
    Task<Sdto> GetIdpDeviceClientId();

    Task<Sdto> GetIdpUrl();

    Task<SessionInfoDto> GetSessionInfo();
}