namespace Dobrasync.Api.BusinessLogic.Services.Core.JwtWhitelist;

public interface IJwtWhitelist
{
    /// <summary>
    ///     Adds the given JWT to whitelist, allowing it to be used for authorization.
    /// </summary>
    /// <param name="jwt"></param>
    public void AllowJwt(string jwt);

    /// <summary>
    ///     Disallows / bans the given JWT, preventing further usage.
    /// </summary>
    /// <param name="jwt"></param>
    public void DisallowJwt(string jwt);
}