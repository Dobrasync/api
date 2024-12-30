using Dobrasync.Api.BusinessLogic.Dtos.User;

namespace Dobrasync.Api.BusinessLogic.Dtos.Auth;

public class SessionInfoDto
{
    public UserDto User { get; set; } = default!;
}