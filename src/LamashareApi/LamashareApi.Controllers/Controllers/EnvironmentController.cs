using Lamashare.BusinessLogic.Dtos.Auth;
using Lamashare.BusinessLogic.Services.Core.AppsettingsProvider;
using LamashareApi.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace LamashareApi.Controllers;

[SwaggerTag("Environment")]
public class EnvironmentController(IAppsettingsProvider apps) : BaseController
{
    #region GET - IDP - Authority

    [HttpGet("idp/authority")]
    [SwaggerOperation(
        Summary = "Get IDP authority",
        OperationId = nameof(GetIdpAuthority)
    )]
    [SwaggerResponse(StatusCodes.Status200OK, nameof(Ok), typeof(Sdto))]
    public async Task<IActionResult> GetIdpAuthority()
    {
        return Ok(new Sdto
        {
            Content = apps.GetAppsettings().Auth.Idp.Authority
        });
    }

    #endregion

    #region GET - IDP - Device - ClientId

    [HttpGet("idp/device/client-id")]
    [SwaggerOperation(
        Summary = "Get IDP client id for device auth",
        OperationId = nameof(GetIdpDeviceClientId)
    )]
    [SwaggerResponse(StatusCodes.Status200OK, nameof(Ok), typeof(Sdto))]
    public async Task<IActionResult> GetIdpDeviceClientId()
    {
        return Ok(new Sdto
        {
            Content = apps.GetAppsettings().Auth.Idp.Device.ClientId
        });
    }

    #endregion
}