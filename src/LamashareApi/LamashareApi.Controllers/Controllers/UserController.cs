using System.Net.Mime;
using Lamashare.BusinessLogic.Dtos.User;
using Lamashare.BusinessLogic.Services.Main.Users;
using LamashareApi.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace LamashareApi.Controllers;

[SwaggerTag("Users")]
public class UserController(IUsersService us) : BaseController
{
    [HttpPost]
    [SwaggerOperation(
        Summary = "Create a new user",
        Description = "Create a new user",
        OperationId = nameof(CreateUser)
    )]
    [SwaggerResponse(StatusCodes.Status200OK, nameof(Ok), typeof(UserDto), MediaTypeNames.Application.Json)]
    public async Task<IActionResult> CreateUser([FromBody] UserCreateDto ucdto)
    {
        return Ok(await us.CreateUser(ucdto));
    }
    
    [HttpGet("username-available/{username}")]
    [SwaggerOperation(
        Summary = "Check username availability",
        Description = "Checks if the given username is not used by anyone else",
        OperationId = nameof(IsUsernameAvailable)
    )]
    [SwaggerResponse(StatusCodes.Status200OK, nameof(Ok), typeof(bool), MediaTypeNames.Application.Json)]
    public async Task<IActionResult> IsUsernameAvailable(string username)
    {
        return Ok(await us.IsUsernameAvailable(username));
    }
    
    [HttpGet("{userId}")]
    [SwaggerOperation(
        Summary = "Get a user with given id",
        Description = "Get a user with given id",
        OperationId = nameof(GetUserById)
    )]
    [SwaggerResponse(StatusCodes.Status200OK, nameof(Ok), typeof(UserDto), MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetUserById(Guid userId)
    {
        return Ok(await us.GetUserById(userId));
    }
}