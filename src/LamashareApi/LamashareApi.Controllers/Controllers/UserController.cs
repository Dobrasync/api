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