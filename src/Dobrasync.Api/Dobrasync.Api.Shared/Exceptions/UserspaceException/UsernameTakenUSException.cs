using Dobrasync.Api.Shared.Localization;

namespace Dobrasync.Api.Shared.Exceptions.UserspaceException;

public class UsernameTakenUSException()
    : UserspaceException(409, LocKeys.ExceptionUsernameTaken.KeyTemplate, "The specified username is already in use.");