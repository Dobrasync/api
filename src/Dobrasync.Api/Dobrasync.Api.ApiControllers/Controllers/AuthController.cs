using System.Security.Claims;
using Lamashare.BusinessLogic.Dtos.Auth;
using Lamashare.BusinessLogic.Services.Main.Auth;
using LamashareApi.Controllers.Base;
using LamashareApi.Shared.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Zitadel.Authentication;

namespace LamashareApi.Controllers;

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