using Dobrasync.Api.Shared.Exceptions.UserspaceException;
using Dobrasync.Core.Common.Models;

namespace Dobrasync.Api.Shared.Extensions;

public static class ApiErrorDtoExtensions
{
    public static ApiErrorDto GetApiErrorDto(this UserspaceException ex)
    {
        return new ApiErrorDto()
        {
            DateTimeUtc = DateTime.UtcNow,
            Message = ex.UserMessage,
            HttpStatusCode = ex.HttpStatusCode
        };
    }
}