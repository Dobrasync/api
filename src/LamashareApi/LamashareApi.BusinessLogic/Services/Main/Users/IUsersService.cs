using Gridify;
using Lamashare.BusinessLogic.Dtos.Auth;
using Lamashare.BusinessLogic.Dtos.Library;
using Lamashare.BusinessLogic.Dtos.User;
using Lamashare.BusinessLogic.Services.Main.UserRegistration;

namespace Lamashare.BusinessLogic.Services.Main.Users;

public interface IUsersService
{
    Task<UserDto> CreateUser(UserCreateDto cdto);
    Task<AuthDto> RegisterUser(RegisterUserDto rdto);
    Task<bool> IsUsernameAvailable(string username);
    Task<UserDto> GetUserByIdAsyncThrows(Guid guid);
    Task<Paging<LibraryDto>> GetAllLibrariesByUser(Guid userId, GridifyQuery query);
    Task<LibraryDto> CreateUserLibrary(Guid userId, LibraryCreateDto dto);
}