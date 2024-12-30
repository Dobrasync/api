using Dobrasync.Api.Shared.Localization;

namespace Dobrasync.Api.Shared.Exceptions.UserspaceException;

public class NotFoundUSException() : UserspaceException(404, LocKeys.ExceptionResourceNotFound.KeyTemplate,
    "The requested resource does not exist.");