using System.Net.Mime;
using Lamashare.BusinessLogic.Dtos.Auth;
using Lamashare.BusinessLogic.Dtos.User;
using Lamashare.BusinessLogic.Services.Main.Auth;
using LamashareApi.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace LamashareApi.Controllers;

[SwaggerTag("Auth")]
public class AuthController(IAuthService authService) : BaseController
{
    [HttpPost("create-auth-token/classic")]
    [SwaggerOperation(
        Summary = "Generate new auth token the classic way",
        Description = "Generates a new auth token for user matching the provided classic credentials.",
        OperationId = nameof(CreateTokenClassic)
    )]
    [SwaggerResponse(StatusCodes.Status200OK, nameof(Ok), typeof(AuthDto), MediaTypeNames.Application.Json)]
    public async Task<IActionResult> CreateTokenClassic([FromBody] ClassicLoginDto dto)
    {
        return Ok(await authService.CreateTokenClassic(dto));
    }
    
    [HttpGet("current-session")]
    [SwaggerOperation(
        Summary = "Returns info about current session",
        Description = "Returns information about the invokers current session.",
        OperationId = nameof(GetSessionInfo)
    )]
    [SwaggerResponse(StatusCodes.Status200OK, nameof(Ok), typeof(SessionInfoDto), MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetSessionInfo()
    {
        return Ok(await authService.GetInvokerSessionInfo());
    }
}