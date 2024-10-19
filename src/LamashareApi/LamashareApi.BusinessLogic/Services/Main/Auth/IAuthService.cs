using Lamashare.BusinessLogic.Dtos.Auth;
using Lamashare.BusinessLogic.Services.Core.Jwt;
using LamashareApi.Database.DB.Entities;
using LamashareApi.Shared.Exceptions.UserspaceException;

namespace Lamashare.BusinessLogic.Services.Main.Auth;

public interface IAuthService
{
    Task<Sdto> Oidc(string code);

    Task<Sdto> GetIdpDeviceClientId();

    Task<Sdto> GetIdpUrl();
    
    /// <summary>
    /// Utility function used for making an auth dto out of JwtClaims.
    /// </summary>
    /// <param name="claims"></param>
    /// <returns></returns>
    AuthDto MakeAuthDto(AuthJwtClaims claims);
    
    /// <summary>
    /// Generates an auth token the classic way (with the invoker provided username and password).
    ///
    /// Basically a technical repres. of the standard way of logging in.
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<AuthDto> CreateTokenClassic(ClassicLoginDto dto);

    /// <summary>
    /// Goes throgh users and tries to find one matching the provided credentials.
    /// </summary>
    /// <param name="loginDto"></param>
    /// <exception cref="InvalidCredentialsUSException">If no matching user can be found</exception>
    /// <returns></returns>
    Task<UserEntity> GetUserMatchingCredentialsThrows(ClassicLoginDto loginDto);
    
    /// <summary>
    /// Returns info about the invokers' session.
    /// </summary>
    /// <returns></returns>
    Task<SessionInfoDto> GetInvokerSessionInfo();
}