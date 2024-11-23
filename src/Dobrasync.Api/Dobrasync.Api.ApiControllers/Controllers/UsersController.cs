using Gridify;
using Lamashare.BusinessLogic.Dtos.Auth;
using Lamashare.BusinessLogic.Dtos.Library;
using Lamashare.BusinessLogic.Dtos.User;
using Lamashare.BusinessLogic.Services.Main.Users;
using LamashareApi.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace LamashareApi.Controllers;

[SwaggerTag("Users")]
public class UsersController(IUsersService usersService) : BaseController
{
    [HttpGet("{userId}")]
    public async Task<ActionResult<UserDto>> GetUserById(Guid userId)
    {
        UserDto res = await usersService.GetUserByIdAsync(userId);
        return Ok(res);
    }
    
    [HttpGet("{userId}/libraries")]
    public async Task<ActionResult<Paging<LibraryDto>>> GetUserLibraries(Guid userId, [FromQuery] GridifyQuery searchQuery)
    {
        Paging<LibraryDto> res = await usersService.GetUserLibraries(userId, searchQuery);
        return Ok(res);
    }
}