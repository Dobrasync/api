using LamashareApi.Shared.Localization;

namespace LamashareApi.Shared.Exceptions.UserspaceException;

public class TransactionTypeUSException() : UserspaceException(403, LocKeys.ExceptionTransactionType.KeyTemplate,
    "The operation is not supported by this transaction type.");