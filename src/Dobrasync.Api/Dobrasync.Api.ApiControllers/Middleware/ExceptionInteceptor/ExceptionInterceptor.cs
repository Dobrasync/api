using Dobrasync.Api.BusinessLogic.Services.Core.Logging;
using Dobrasync.Api.Shared.Exceptions.UserspaceException;
using Dobrasync.Api.Shared.Extensions;
using Dobrasync.Core.Common.Models;

namespace Dobrasync.Api.ApiControllers.Middleware.ExceptionInteceptor;

public class ExceptionInterceptor(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext httpContext, ILoggingService logger)
    {
        try
        {
            // Pass request up the chain
            await next(httpContext);
        }
        catch (Exception ex)
        {
            await WriteResponse(ex, httpContext, logger);
        }
    }

    public async Task WriteResponse(Exception ex, HttpContext context, ILoggingService logger)
    {
        ApiErrorDto? errorDto = null;
        if (ex is UserspaceException userspaceException)
        {
            errorDto = userspaceException.GetApiErrorDto();
            logger.LogDebug($"Userspace error during request: {ex.StackTrace}");
        }
        else
        {
            errorDto = new()
            {
                HttpStatusCode = 500,
                Message = "Internal server error",
            };
            logger.LogError($"Internal error during request: {ex.StackTrace}");
        }

        context.Response.StatusCode = errorDto.HttpStatusCode;
        await context.Response.WriteAsJsonAsync(errorDto);
    }
}