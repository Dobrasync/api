using System.Net.Mime;
using System.Security.Claims;
using Lamashare.BusinessLogic.Dtos.Auth;
using Lamashare.BusinessLogic.Dtos.User;
using Lamashare.BusinessLogic.Services.Main.Auth;
using LamashareApi.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Zitadel.Authentication;

namespace LamashareApi.Controllers;

[SwaggerTag("Auth")]
public class AuthController(IAuthService authService) : BaseController
{
    #region Test
    #region GET - JWT
    [HttpGet("jwt")]
    [Authorize(AuthenticationSchemes = "ZITADEL_JWT")]
    public async Task<IActionResult> GetJwt()
    {
        return Ok(Result());
    }
    #endregion
    #region GET - Basic
    [HttpGet("basic")]
    [Authorize(AuthenticationSchemes = "ZITADEL_BASIC")]
    public async Task<IActionResult> GetBasic()
    {
        return Ok(Result());
    }
    #endregion
    #region GET - Fake
    [HttpGet("fake")]
    [Authorize(AuthenticationSchemes = "ZITADEL_FAKE")]
    public async Task<IActionResult> GetFake()
    {
        return Ok(Result());
    }
    #endregion
    
    private object Result() => new
    {
        Ping = "Pong",
        Timestamp = DateTime.Now,
        AuthType = User.Identity?.AuthenticationType,
        UserName = User.Identity?.Name,
        UserId = User.FindFirstValue(OidcClaimTypes.Subject),
        Claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList(),
        IsInAdminRole = User.IsInRole("Admin"),
        IsInUserRole = User.IsInRole("User"),
    };
    #endregion
    
    #region GET - Session info
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
    #endregion
}