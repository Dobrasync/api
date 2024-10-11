using Lamashare.BusinessLogic.Dtos.File;
using LamashareApi.Database.Enums;

namespace LamashareApi.Database.DB.Entities;

public class FileTransaction : BaseEntity
{
    public Guid Id { get; set; }
    public DateTimeOffset DateCreated { get; set; }
    public DateTimeOffset DateModified { get; set; }
    public File File { get; set; } = default!;
    public EFileTransactionStatus Status { get; set; }
    public EFileTransactionType Type { get; set; }
    public string? ExpectedChecksum { get; set; }
    
    /// <summary>
    /// Blocks required to assemble final file.
    /// </summary>
    public List<string>? TotalBlocks { get; set; } = new();
    
    /// <summary>
    /// Blocks the client needs to send before finalizing
    /// </summary>
    public List<string>? RequiredBlocks { get; set; } = new();
    
    /// <summary>
    /// Blocks the client has sent.
    /// </summary>
    public List<string>? ReceivedBlocks { get; set; } = new();
    
    public DateTimeOffset DateCreatedFile { get; set; }
    public DateTimeOffset DateModifiedFile { get; set; }
}