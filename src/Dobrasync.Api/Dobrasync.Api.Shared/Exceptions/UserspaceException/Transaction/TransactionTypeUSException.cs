using Dobrasync.Api.Shared.Localization;

namespace Dobrasync.Api.Shared.Exceptions.UserspaceException.Transaction;

public class TransactionTypeUSException() : UserspaceException(403, LocKeys.ExceptionTransactionType.KeyTemplate,
    "The operation is not supported by this transaction type.");