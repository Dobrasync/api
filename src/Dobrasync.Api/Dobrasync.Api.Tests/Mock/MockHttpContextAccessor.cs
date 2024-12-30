using Microsoft.AspNetCore.Http;

namespace Dobrasync.Api.Tests.Mock;

public class MockHttpContextAccessor : IHttpContextAccessor
{
    public HttpContext? HttpContext { get; set; }
}