namespace Dobrasync.Api.Shared.Constants;

public static class LibraryUtil
{
    public static string GetLibraryDirectory(Guid libraryId, string storageDirectory)
    {
        return Path.Join(storageDirectory, libraryId.ToString());
    }
}