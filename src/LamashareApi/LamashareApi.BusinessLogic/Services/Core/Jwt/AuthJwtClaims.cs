using System.Security.Claims;
using LamashareApi.Database.DB.Entities;
using LamashareApi.Shared.Permissions;

namespace Lamashare.BusinessLogic.Services.Core.Jwt;

public class AuthJwtClaims
{
    public Guid UserId { get; set; }
    public EUserRole Role { get; set; }
    
    public List<Claim> ToClaims()
    {
        return new List<Claim>
        {
            new Claim(AuthJwtConsts.ClaimNameUserId, UserId.ToString()),
            new Claim(AuthJwtConsts.ClaimNameRole, Role.ToString()),
        };
    }

    public static AuthJwtClaims FromUser(UserEntity userEntity)
    {
        return new()
        {
            UserId = userEntity.Id,
            Role = userEntity.Role
        };
    }

    public static AuthJwtClaims? FromClaims(List<Claim> claims)
    {
        string? guidRaw = claims.FirstOrDefault(x => x.Type == AuthJwtConsts.ClaimNameUserId)?.Value;
        string? roleRaw = claims.FirstOrDefault(x => x.Type == AuthJwtConsts.ClaimNameRole)?.Value;
        
        bool guidOk = Guid.TryParse(guidRaw, out Guid userGuid);
        bool roleOk =  Enum.TryParse<EUserRole>(roleRaw, out EUserRole role);
        
        if (!guidOk || !roleOk) return null;

        return new()
        {
            UserId = userGuid,
            Role = role,
        };
    }
}