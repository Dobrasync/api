namespace LamashareApi.Shared.Localization;

public static class LocKeys
{
    public static readonly LocKey ExceptionBadInput = new()
    {
        Key = "exception.badInput"
    };

    public static readonly LocKey ExceptionUsernameTaken = new()
    {
        Key = "exception.usernameTaken"
    };

    public static readonly LocKey ExceptionResourceNotFound = new()
    {
        Key = "exception.notFound"
    };

    #region Transaction

    public static readonly LocKey ExceptionTransactionAlreadyComplete = new()
    {
        Key = "exception.transactionAlreadyComplete"
    };

    public static readonly LocKey ExceptionTransactionBlockMismatch = new()
    {
        Key = "exception.transactionBlockMismatch"
    };

    public static readonly LocKey ExceptionTransactionConflict = new()
    {
        Key = "exception.transactionConflict"
    };

    public static readonly LocKey ExceptionTransactionType = new()
    {
        Key = "exception.transactionType"
    };

    #endregion
}