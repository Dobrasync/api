using Lamashare.BusinessLogic.Dtos.User;

namespace Lamashare.BusinessLogic.Services.Main.Users;

public interface IUsersService
{
    Task<UserDto> GetUserById(Guid guid);
}