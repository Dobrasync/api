using Lamashare.BusinessLogic.Services.Core.Jwt;
using LamashareApi.Database.DB.Entities;
using LamashareApi.Database.Repos;
using LamashareApi.Shared.Constants;
using LamashareApi.Shared.Exceptions.UserspaceException;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Lamashare.BusinessLogic.Services.Main.InvokerService;

public class InvokerService(IHttpContextAccessor httpContextAccessor, IJwtService jwtService, IRepoWrapper repoWrap) : IInvokerService
{
    public async Task<UserEntity?> TryGetInvokerAsync()
    {
        try
        {
            UserEntity? user = await GetInvokerAsyncThrows();
            return user;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<UserEntity> GetInvokerAsyncThrows()
    {
        //AuthJwtClaims claims = GetInvokerAuthJwtClaimsThrows();

        //User? user = await repoWrap.UserRepo.GetByIdAsync(claims.UserId);
        UserEntity? user = await repoWrap.UserRepo.QueryAll().FirstOrDefaultAsync();
        
        if (user == null)
            throw new InvalidAuthTokenUSException();

        return user;
    }

    public AuthJwtClaims GetInvokerAuthJwtClaimsThrows()
    {
        string? authJwt = httpContextAccessor?.HttpContext?.Request?.Headers[EtcConstants.AuthHeaderName];
        if (string.IsNullOrEmpty(authJwt))
            throw new InvalidAuthTokenUSException();

        string withoutBearerPrefix = authJwt.Replace("Bearer ", "");
        
        AuthJwtClaims decodeAuthClaims = jwtService.DecodeAuthJwt(withoutBearerPrefix);
        return decodeAuthClaims;
    }
}