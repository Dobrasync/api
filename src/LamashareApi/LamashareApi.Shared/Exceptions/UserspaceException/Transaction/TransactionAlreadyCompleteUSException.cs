using LamashareApi.Shared.Localization;

namespace LamashareApi.Shared.Exceptions.UserspaceException;

public class TransactionAlreadyCompleteUSException() : UserspaceException(400,
    LocKeys.ExceptionTransactionAlreadyComplete.KeyTemplate, "The transaction has already been completed.");