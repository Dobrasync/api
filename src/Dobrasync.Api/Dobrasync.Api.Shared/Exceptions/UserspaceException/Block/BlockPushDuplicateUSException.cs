namespace Dobrasync.Api.Shared.Exceptions.UserspaceException.Block;

public class BlockPushDuplicateUSException() : UserspaceException(409, "This block already exists.",
    "The block already exists and does not need to be pushed again.");