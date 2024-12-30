using Dobrasync.Api.ApiControllers.Controllers.Base;
using Dobrasync.Api.BusinessLogic.Dtos.Library;
using Dobrasync.Api.BusinessLogic.Dtos.User;
using Dobrasync.Api.BusinessLogic.Services.Main.Users;
using Gridify;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Dobrasync.Api.ApiControllers.Controllers;

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