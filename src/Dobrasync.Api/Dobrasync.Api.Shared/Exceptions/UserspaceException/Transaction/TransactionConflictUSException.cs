using LamashareApi.Shared.Localization;

namespace LamashareApi.Shared.Exceptions.UserspaceException;

public class TransactionConflictUSException() : UserspaceException(409,
    LocKeys.ExceptionTransactionConflict.KeyTemplate,
    "There is another ongoing transaction with the same target file.");