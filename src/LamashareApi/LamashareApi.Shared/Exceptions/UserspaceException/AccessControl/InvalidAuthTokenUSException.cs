namespace LamashareApi.Shared.Exceptions.UserspaceException;

public class InvalidAuthTokenUSException : UserspaceException
{
    public InvalidAuthTokenUSException() : base(401, "", "No valid authentication token was provided.")
    {
    }
}