namespace Dobrasync.Api.Shared.Exceptions.UserspaceException.AccessControl;

public class InvalidAuthTokenUSException : UserspaceException
{
    public InvalidAuthTokenUSException() : base(401, "", "No valid authentication token was provided.")
    {
    }
}