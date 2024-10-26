namespace LamashareApi.Shared.Exceptions.UserspaceException;

public class InvalidCredentialsUSException : UserspaceException
{
    public InvalidCredentialsUSException(string msg) : base(403, msg, "The provided credentials are invalid.")
    {
    }
}