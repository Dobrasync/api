using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using Lamashare.BusinessLogic.Dtos.Auth;
using Lamashare.BusinessLogic.Dtos.User;
using Lamashare.BusinessLogic.Services.Core.Jwt;
using Lamashare.BusinessLogic.Services.Main.InvokerService;
using LamashareApi.Database.DB.Entities;
using LamashareApi.Database.Repos;
using LamashareApi.Shared.Exceptions.UserspaceException;
using LamashareApi.Shared.Password;
using Microsoft.EntityFrameworkCore;

namespace Lamashare.BusinessLogic.Services.Main.Auth;

public class AuthService(IJwtService jwtService, IInvokerService invokerService, IRepoWrapper repoWrap, IMapper mapper) : IAuthService
{
    public AuthDto MakeAuthDto(AuthJwtClaims claims)
    {
        (string jwtString, JwtSecurityToken token) = jwtService.GenerateAuthJwt(claims);
        return new()
        {
            AuthToken = jwtString,
            ExpiresUtc = token.ValidTo
        };
    }

    public async Task<AuthDto> CreateTokenClassic(ClassicLoginDto dto)
    {
        User invoker = await GetUserMatchingCredentialsThrows(dto);

        AuthJwtClaims claims = AuthJwtClaims.FromUser(invoker);
        AuthDto authTokenDto = MakeAuthDto(claims);

        return authTokenDto;
    }

    public async Task<User> GetUserMatchingCredentialsThrows(ClassicLoginDto loginDto)
    {
        User? usernameMatch = await repoWrap.UserRepo.QueryAll()
            .FirstOrDefaultAsync(x => x.Username.ToLower() == loginDto.Username.ToLower());

        if (usernameMatch == null) 
            throw new InvalidCredentialsUSException("");
        
        if (!SecretHasher.Verify(loginDto.Password, usernameMatch.Password))
            throw new InvalidCredentialsUSException("");

        return usernameMatch;
    }

    public async Task<SessionInfoDto> GetInvokerSessionInfo()
    {
        User invoker = await invokerService.GetInvokerAsyncThrows();

        return new()
        {
            User = mapper.Map<UserDto>(invoker)
        };
    }
}