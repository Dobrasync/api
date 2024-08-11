using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace LamashareApi.Controllers.Base;

[ApiVersion("1.0")]
[Route("v{version:apiVersion}/{controller}")]
[ApiController]
public class BaseController : ControllerBase
{
}