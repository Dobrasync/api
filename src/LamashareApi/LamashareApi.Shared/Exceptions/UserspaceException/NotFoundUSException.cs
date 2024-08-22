namespace LamashareApi.Shared.Exceptions.UserspaceException;

public class NotFoundUSException : UserspaceException
{
    public NotFoundUSException(string message) : base(404, message, "The requested resource does not exist.")
    {
    }
}