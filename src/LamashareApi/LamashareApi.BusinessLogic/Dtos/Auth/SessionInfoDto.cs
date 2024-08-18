using System.IdentityModel.Tokens.Jwt;
using Lamashare.BusinessLogic.Dtos.User;
using Lamashare.BusinessLogic.Services.Core.Jwt;

namespace Lamashare.BusinessLogic.Dtos.Auth;

public class SessionInfoDto
{
    public UserDto User { get; set; } = default!;
}