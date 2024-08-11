using LamashareApi.Shared.Exceptions.ApiExceptions;

namespace Lamashare.BusinessLogic.Dtos.ErrorDto;

/// <summary>
///     Returned from API whenever there is an error.
/// </summary>
public class ErrorDto
{
    public DateTime DateTimeUtc { get; set; }
    public int HttpStatusCode { get; set; }
    public string Message { get; set; } = default!;

    public static ErrorDto CreateFromUserspaceException(UserspaceException ex)
    {
        return new ErrorDto
        {
            DateTimeUtc = DateTime.UtcNow,
            Message = ex.UserMessage,
            HttpStatusCode = ex.HttpStatusCode
        };
    }

    public static ErrorDto CreateGenericInternalServerError()
    {
        return new ErrorDto
        {
            HttpStatusCode = 500,
            Message = "Unspecified internal server error",
            DateTimeUtc = DateTime.UtcNow
        };
    }
}