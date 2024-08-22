namespace LamashareApi.Shared.Exceptions.UserspaceException;

public class LibraryNameConflictUSException : UserspaceException
{
    public LibraryNameConflictUSException() : base(409, "", "A library with same name already exists.")
    {
    }
}