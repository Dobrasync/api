namespace LamashareApi.Shared.Localization;

public static class LocKeys
{
    public static readonly LocKey ExceptionUsernameTaken = new()
    {
        Key ="exception.usernameTaken"
    };
    public static readonly LocKey ExceptionResourceNotFound = new()
    {
        Key ="exception.notFound"
    };
    public static readonly LocKey ExceptionTransactionAlreadyComplete = new()
    {
        Key ="exception.transactionAlreadyComplete"
    };
    
    public static readonly LocKey ExceptionTransactionBlockMismatch = new()
    {
        Key ="exception.transactionBlockMismatch"
    };
}