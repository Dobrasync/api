namespace Lamashare.BusinessLogic.Dtos.File;

public class BlockPushDto
{
    public Guid TransactionId { get; set; }
    public Guid LibraryId { get; set; }
    public string Checksum { get; set; } = default!;
    public byte[] Content { get; set; } = default!;
    public long Offset { get; set; }
    public int Size { get; set; }
}