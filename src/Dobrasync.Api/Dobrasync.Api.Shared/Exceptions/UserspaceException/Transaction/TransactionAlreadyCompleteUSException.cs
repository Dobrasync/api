using Dobrasync.Api.Shared.Localization;

namespace Dobrasync.Api.Shared.Exceptions.UserspaceException.Transaction;

public class TransactionAlreadyCompleteUSException() : UserspaceException(400,
    LocKeys.ExceptionTransactionAlreadyComplete.KeyTemplate, "The transaction has already been completed.");