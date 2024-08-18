using Lamashare.BusinessLogic.Dtos.Auth;
using Lamashare.BusinessLogic.Dtos.User;
using Lamashare.BusinessLogic.Services.Main.UserRegistration;

namespace Lamashare.BusinessLogic.Services.Main.Users;

public interface IUsersService
{
    Task<UserDto> CreateUser(UserCreateDto cdto);
    Task<AuthDto> RegisterUser(RegisterUserDto rdto);
    Task<bool> IsUsernameAvailable(string username);
    Task<UserDto> GetUserById(Guid guid);
}