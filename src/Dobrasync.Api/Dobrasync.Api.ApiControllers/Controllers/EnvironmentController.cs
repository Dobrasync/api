using Dobrasync.Api.ApiControllers.Controllers.Base;
using Dobrasync.Api.BusinessLogic.Dtos.Auth;
using Dobrasync.Api.BusinessLogic.Services.Core.AppsettingsProvider;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Dobrasync.Api.ApiControllers.Controllers;

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
    public IActionResult GetIdpAuthority()
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
    public IActionResult GetIdpDeviceClientId()
    {
        return Ok(new Sdto
        {
            Content = apps.GetAppsettings().Auth.Idp.Device.ClientId
        });
    }

    #endregion
}