using Dobrasync.Api.BusinessLogic.Dtos.Library;
using Dobrasync.Api.BusinessLogic.Dtos.User;
using Gridify;

namespace Dobrasync.Api.BusinessLogic.Services.Main.Users;

public interface IUsersService
{
    /// <summary>
    /// Get a user by their id.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public Task<UserDto> GetUserByIdAsync(Guid userId);
    
    /// <summary>
    /// Gets all libraries of a user.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="searchQuery"></param>
    /// <returns></returns>
    public Task<Paging<LibraryDto>>  GetUserLibraries(Guid userId, GridifyQuery searchQuery);
}