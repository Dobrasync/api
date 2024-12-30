using Dobrasync.Api.ApiControllers.Controllers.Base;
using Dobrasync.Api.BusinessLogic.Dtos.Auth;
using Dobrasync.Api.BusinessLogic.Services.Main.Auth;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Dobrasync.Api.ApiControllers.Controllers;

[SwaggerTag("Auth")]
public class AuthController(IAuthService authService) : BaseController
{
    [HttpGet("session-info")]
    [SwaggerOperation(
        OperationId = nameof(GetSessionInfo)
    )]
    public async Task<ActionResult<SessionInfoDto>> GetSessionInfo()
    {
        SessionInfoDto dto = await authService.GetSessionInfo();
        return Ok(dto);
    }
}