using Dobrasync.Api.Shared.Localization;

namespace Dobrasync.Api.Shared.Exceptions.UserspaceException.Transaction;

public class TransactionBlockMismatchUSException() : UserspaceException(400,
    LocKeys.ExceptionTransactionBlockMismatch.KeyTemplate, "The received blocks do not match the expected blocks.");