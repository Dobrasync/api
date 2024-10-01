using Lamashare.BusinessLogic.Dtos.File;
using LamashareApi.Database.Enums;

namespace LamashareApi.Database.DB.Entities;

public class FileTransaction : BaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime LastUpdatedOn { get; set; }
    public File File { get; set; } = default!;
    public EFileTransactionStatus Status { get; set; }
    public EFileTransactionType Type { get; set; }
    public string ExpectedChecksum { get; set; } = default!;
    public List<string> ExpectedBlocks { get; set; } = new();
    public List<string> ReceivedBlocks { get; set; } = new();
}