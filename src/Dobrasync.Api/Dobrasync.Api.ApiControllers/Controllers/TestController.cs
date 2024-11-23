using Lamashare.BusinessLogic.Dtos.Auth;
using Lamashare.BusinessLogic.Dtos.ErrorDto;
using Lamashare.BusinessLogic.Services.Core.AppsettingsProvider;
using LamashareApi.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace LamashareApi.Controllers;

/// <summary>
/// Some of these endpoints are only present so codegen on clients
/// also includes otherwise excluded dtos.
/// </summary>
/// <param name="apps"></param>
[SwaggerTag("Test")]
public class TestController(IAppsettingsProvider apps) : BaseController
{
    [HttpGet("error-dto-example")]
    public ActionResult<ErrorDto> GetExampleErrorDto()
    {
        return Ok(new ErrorDto()
        {
            Message = "Test",
            HttpStatusCode = 400,
            DateTimeUtc = DateTime.UtcNow,
        });
    }
}