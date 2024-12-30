using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Dobrasync.Api.ApiControllers.Controllers.Base;

[ApiVersion("1")]
[Route("v{version:apiVersion}/[controller]")]
[ApiController]
public class BaseController : ControllerBase
{
}