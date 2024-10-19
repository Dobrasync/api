namespace Lamashare.BusinessLogic.Dtos.File;

public class FileTransactionDto
{
    public Guid Id { get; set; }
    public Guid FileId { get; set; }
    public EFileTransactionType Type { get; set; }

    /// <summary>
    ///     List of blocks that are not present on the remote
    ///     and therefore need to be sent by client during
    ///     transaction.
    ///     Irrelevant on PULL.
    /// </summary>
    public List<string> RequiredBlocks { get; set; } = new();
}