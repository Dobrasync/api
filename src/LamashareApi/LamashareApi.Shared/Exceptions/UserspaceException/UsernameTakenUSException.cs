namespace LamashareApi.Shared.Exceptions.UserspaceException;

public class UsernameTakenUSException(string message)
    : UserspaceException(409, message, "The specified username is already in use.");