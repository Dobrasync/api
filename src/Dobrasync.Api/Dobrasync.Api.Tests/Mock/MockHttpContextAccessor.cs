using Microsoft.AspNetCore.Http;

namespace LamashareApi.Tests.Mock;

public class MockHttpContextAccessor : IHttpContextAccessor
{
    public HttpContext? HttpContext { get; set; }
}