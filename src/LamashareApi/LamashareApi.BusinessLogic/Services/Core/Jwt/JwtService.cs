using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Lamashare.BusinessLogic.Services.Core.AppsettingsProvider;
using LamashareApi.Shared.Exceptions.UserspaceException;
using Microsoft.IdentityModel.Tokens;

namespace Lamashare.BusinessLogic.Services.Core.Jwt;

public class JwtService(IAppsettingsProvider appsProvider) : IJwtService
{
    public (string, JwtSecurityToken) GenerateAuthJwt(AuthJwtClaims claims)
    {
        var apps = appsProvider.GetAppsettings();
        return GenerateJwt(
            apps.Auth.AuthJwt.Secret,
            apps.Auth.AuthJwt.Issuer,
            apps.Auth.AuthJwt.Audience,
            apps.Auth.AuthJwt.LifetimeMinutes,
            claims.ToClaims()
        );
    }
    
    public (string, JwtSecurityToken) GenerateJwt(string secret, string issuer, string audience, int expiryMinutes, List<Claim> claims)
    {
        JwtSecurityToken token = GenerateJwtSecurityToken(secret, issuer, audience, expiryMinutes, claims);
        return (new JwtSecurityTokenHandler().WriteToken(token), token);
    }
    
    public JwtSecurityToken GenerateJwtSecurityToken(string secret, string issuer, string audience, int expiryMinutes, List<Claim> claims)
    {
        SymmetricSecurityKey key = new(Convert.FromBase64String(secret));
        SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);
        DateTime expires = DateTime.UtcNow.AddMinutes(expiryMinutes);

        JwtSecurityToken token = new(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );
        
        return token;
    }
    
    public JwtSecurityToken DecodeJwtSecurityToken(string secret, string issuer, string audience, string rawJwtString)
    {
        if (string.IsNullOrEmpty(rawJwtString)) throw new InvalidAuthTokenUSException();
        
        SymmetricSecurityKey key = new(Convert.FromBase64String(secret));
        JwtSecurityTokenHandler handler = new();

        TokenValidationParameters tokenParams = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = key,
        };

        handler.ValidateToken(rawJwtString, tokenParams, out SecurityToken validatedToken);
        
        return (JwtSecurityToken)validatedToken;
    }

    public AuthJwtClaims DecodeAuthJwt(string rawJwt)
    {
        var apps = appsProvider.GetAppsettings();
        JwtSecurityToken token = DecodeJwtSecurityToken(
            apps.Auth.AuthJwt.Secret,
            apps.Auth.AuthJwt.Issuer,
            apps.Auth.AuthJwt.Audience,
            rawJwt);

        var claims = AuthJwtClaims.FromClaims(token.Claims.ToList());
        if (claims == null) 
            throw new InvalidAuthTokenUSException();

        return claims;
    }
}