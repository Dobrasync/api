using Dobrasync.Api.Database.DB.Entities.Base;
using Dobrasync.Api.Database.Enums;

namespace Dobrasync.Api.Database.DB.Entities;

public class FileTransactionEntity : BaseEntity
{
    public DateTimeOffset DateCreated { get; set; }
    public DateTimeOffset DateModified { get; set; }
    public FileEntity File { get; set; } = default!;
    public EFileTransactionStatus Status { get; set; }
    public EFileTransactionType Type { get; set; }
    public string? ExpectedChecksum { get; set; }

    /// <summary>
    ///     Blocks required to assemble final file.
    /// </summary>
    public List<string>? TotalBlocks { get; set; } = new();

    /// <summary>
    ///     Blocks the client needs to send before finalizing
    /// </summary>
    public List<string>? RequiredBlocks { get; set; } = new();

    /// <summary>
    ///     Blocks the client has sent.
    /// </summary>
    public List<string>? ReceivedBlocks { get; set; } = new();

    public DateTimeOffset DateCreatedFile { get; set; }
    public DateTimeOffset DateModifiedFile { get; set; }
}