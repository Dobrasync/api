using LamashareApi.Shared.Localization;

namespace LamashareApi.Shared.Exceptions.UserspaceException;

public class TransactionBlockMismatchUSException() : UserspaceException(400,
    LocKeys.ExceptionTransactionBlockMismatch.KeyTemplate, "The received blocks do not match the expected blocks.");