using Lamashare.BusinessLogic.Services.Core.Jwt;
using LamashareApi.Database.DB.Entities;
using LamashareApi.Database.Repos;
using LamashareApi.Shared.Constants;
using LamashareApi.Shared.Exceptions.UserspaceException;
using Microsoft.AspNetCore.Http;

namespace Lamashare.BusinessLogic.Services.Main.InvokerService;

public class InvokerService(IHttpContextAccessor httpContextAccessor, IJwtService jwtService, IRepoWrapper repoWrap) : IInvokerService
{
    public async Task<User?> TryGetInvokerAsync()
    {
        try
        {
            User? user = await GetInvokerAsyncThrows();
            return user;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<User> GetInvokerAsyncThrows()
    {
        AuthJwtClaims claims = GetInvokerAuthJwtClaimsThrows();

        User? user = await repoWrap.UserRepo.GetByIdAsync(claims.UserId);

        if (user == null)
            throw new InvalidAuthTokenUSException();

        return user;
    }

    public AuthJwtClaims GetInvokerAuthJwtClaimsThrows()
    {
        string? authJwt = httpContextAccessor?.HttpContext?.Request?.Headers[EtcConstants.AuthHeaderName];
        if (string.IsNullOrEmpty(authJwt))
            throw new InvalidAuthTokenUSException();
        
        AuthJwtClaims decodeAuthClaims = jwtService.DecodeAuthJwt(authJwt);
        return decodeAuthClaims;
    }
}