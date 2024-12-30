namespace Dobrasync.Api.Shared.Exceptions.UserspaceException.AccessControl;

public class UnauthorizedUSException() : UserspaceException(403, "Unauthorized",
    "Invoker tried to access a resource they do not have access to.");