using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Lamashare.BusinessLogic.Services.Core.Jwt;

public interface IJwtService
{
    (string, JwtSecurityToken) GenerateAuthJwt(AuthJwtClaims claims);

    (string, JwtSecurityToken) GenerateJwt(string secret, string issuer, string audience, int expiryMinutes, List<Claim> claims);

    JwtSecurityToken GenerateJwtSecurityToken(string secret, string issuer, string audience, int expiryMinutes,
        List<Claim> claims);
    
    AuthJwtClaims DecodeAuthJwt(string rawJwt);
}