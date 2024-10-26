using System.Security.Claims;
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
    #region Test

    #region GET - Basic

    [HttpGet("basic")]
    [Authorize(AuthenticationSchemes = AuthSchemes.Basic)]
    public async Task<IActionResult> GetBasic()
    {
        return Ok(Result());
    }

    #endregion

    private object Result()
    {
        return new
        {
            Ping = "Pong",
            Timestamp = DateTime.Now,
            AuthType = User.Identity?.AuthenticationType,
            UserName = User.Identity?.Name,
            UserId = User.FindFirstValue(OidcClaimTypes.Subject),
            Claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList(),
            IsInAdminRole = User.IsInRole("Admin"),
            IsInUserRole = User.IsInRole("User")
        };
    }

    #endregion
}