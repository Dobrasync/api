using Lamashare.BusinessLogic.Dtos.User;

namespace Lamashare.BusinessLogic.Services.Main.Users;

public interface IUsersService
{
    Task<UserDto> CreateUser(UserCreateDto cdto);
    Task<bool> IsUsernameAvailable(string username);
    Task<UserDto> GetUserById(Guid guid);
}