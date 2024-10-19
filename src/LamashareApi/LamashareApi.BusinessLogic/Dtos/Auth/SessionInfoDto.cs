using Lamashare.BusinessLogic.Dtos.User;

namespace Lamashare.BusinessLogic.Dtos.Auth;

public class SessionInfoDto
{
    public UserDto User { get; set; } = default!;
}