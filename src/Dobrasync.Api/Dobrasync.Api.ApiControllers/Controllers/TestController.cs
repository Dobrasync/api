using Dobrasync.Api.ApiControllers.Controllers.Base;
using Dobrasync.Api.BusinessLogic.Services.Core.AppsettingsProvider;
using Dobrasync.Core.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Dobrasync.Api.ApiControllers.Controllers;

/// <summary>
/// Some of these endpoints are only present so codegen on clients
/// also includes otherwise excluded dtos.
/// </summary>
[SwaggerTag("Test")]
public class TestController : BaseController
{
    [HttpGet("error-dto-example")]
    public ActionResult<ApiErrorDto> GetExampleErrorDto()
    {
        return Ok(new ApiErrorDto()
        {
            Message = "Test",
            HttpStatusCode = 400,
            DateTimeUtc = DateTime.UtcNow,
        });
    }
}