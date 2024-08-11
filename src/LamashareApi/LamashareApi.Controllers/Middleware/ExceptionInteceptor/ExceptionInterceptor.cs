using Lamashare.BusinessLogic.Dtos.ErrorDto;
using LamashareApi.Shared.Exceptions.ApiExceptions;

namespace LamashareApi.Middleware.ExceptionInteceptor;

public class ExceptionInterceptor(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            // Pass request up the chain
            await next(httpContext);
        }
        catch (Exception ex)
        {
            await WriteResponse(ex, httpContext);
        }
    }

    public async Task WriteResponse(Exception ex, HttpContext context)
    {
        ErrorDto? errorDto = null;
        if (ex is UserspaceException userspaceException)
            errorDto = ErrorDto.CreateFromUserspaceException(userspaceException);
        else
            errorDto = ErrorDto.CreateGenericInternalServerError();

        context.Response.StatusCode = errorDto.HttpStatusCode;
        await context.Response.WriteAsJsonAsync(errorDto);
    }
}