using System.Net.Mime;
using Gridify;
using Lamashare.BusinessLogic.Dtos.Auth;
using Lamashare.BusinessLogic.Dtos.Library;
using Lamashare.BusinessLogic.Dtos.User;
using Lamashare.BusinessLogic.Services.Main.UserRegistration;
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
    
    [HttpPost("register")]
    [SwaggerOperation(
        Summary = "Register as new user",
        Description = "Register and create a new account",
        OperationId = nameof(RegisterUser)
    )]
    [SwaggerResponse(StatusCodes.Status200OK, nameof(Ok), typeof(AuthDto), MediaTypeNames.Application.Json)]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto rdto)
    {
        return Ok(await us.RegisterUser(rdto));
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
        return Ok(await us.GetUserByIdAsyncThrows(userId));
    }
    
    [HttpGet("{userId}/libraries")]
    [SwaggerOperation(
        Summary = "Gets all libraries of user",
        Description = "Gets all libraries of user",
        OperationId = nameof(GetAllLibrariesOfUser)
    )]
    [SwaggerResponse(StatusCodes.Status200OK, nameof(Ok), typeof(Paging<LibraryDto>), MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetAllLibrariesOfUser(Guid userId, [FromQuery] GridifyQuery searchQuery)
    {
        return Ok(await us.GetAllLibrariesByUser(userId, searchQuery));
    }
    
    [HttpPost("{userId}/library")]
    [SwaggerOperation(
        Summary = "Create new library for user",
        Description = "Creates a new library for the user",
        OperationId = nameof(CreateUserLibrary)
    )]
    [SwaggerResponse(StatusCodes.Status200OK, nameof(Ok), typeof(Paging<LibraryDto>), MediaTypeNames.Application.Json)]
    public async Task<IActionResult> CreateUserLibrary(Guid userId, LibraryCreateDto libraryCreateDto)
    {
        return Ok(await us.CreateUserLibrary(userId, libraryCreateDto));
    }
}