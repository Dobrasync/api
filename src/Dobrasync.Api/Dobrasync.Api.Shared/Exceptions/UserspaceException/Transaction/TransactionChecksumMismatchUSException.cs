namespace Dobrasync.Api.Shared.Exceptions.UserspaceException.Transaction;

public class TransactionChecksumMismatchUSException() : UserspaceException(400, "",
    "The assembled files checksum is different from the expected checksum.");