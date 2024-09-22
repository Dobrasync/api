using LamashareApi.Shared.Localization;

namespace LamashareApi.Shared.Exceptions.UserspaceException;

public class NotFoundUSException() : UserspaceException(404, LocKeys.ExceptionNotFound.KeyTemplate,
    "The requested resource does not exist.");