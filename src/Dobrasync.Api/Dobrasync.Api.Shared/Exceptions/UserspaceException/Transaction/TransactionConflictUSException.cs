using Dobrasync.Api.Shared.Localization;

namespace Dobrasync.Api.Shared.Exceptions.UserspaceException.Transaction;

public class TransactionConflictUSException() : UserspaceException(409,
    LocKeys.ExceptionTransactionConflict.KeyTemplate,
    "There is another ongoing transaction with the same target file.");