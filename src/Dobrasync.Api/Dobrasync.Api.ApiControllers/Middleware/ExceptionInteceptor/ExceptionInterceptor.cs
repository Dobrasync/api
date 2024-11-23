using Lamashare.BusinessLogic.Dtos.ErrorDto;
using Lamashare.BusinessLogic.Services.Core;
using LamashareApi.Shared.Exceptions.UserspaceException;

namespace LamashareApi.Middleware.ExceptionInteceptor;

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
        ErrorDto? errorDto = null;
        if (ex is UserspaceException userspaceException)
        {
            errorDto = ErrorDto.CreateFromUserspaceException(userspaceException);
            logger.LogDebug($"Userspace error during request: {ex.StackTrace}");
        }
        else
        {
            errorDto = ErrorDto.CreateGenericInternalServerError();
            logger.LogError($"Internal error during request: {ex.StackTrace}");
        }

        context.Response.StatusCode = errorDto.HttpStatusCode;
        await context.Response.WriteAsJsonAsync(errorDto);
    }
}