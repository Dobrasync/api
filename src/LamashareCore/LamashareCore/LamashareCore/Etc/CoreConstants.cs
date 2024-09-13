namespace LamashareCore.Etc;

public static class CoreConstants
{
    public static readonly string API_URL_BLOCKSUBMIT = "/blocks/submit";
    public static string GetApiUrlFileBlocklist(Guid libraryId) => $"/library/{libraryId}/blocklist";
}