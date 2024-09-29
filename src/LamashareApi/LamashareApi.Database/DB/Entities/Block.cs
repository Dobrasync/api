namespace LamashareApi.Database.DB.Entities;

public class Block : BaseEntity
{
    public Guid Id { get; set; }
    public string Checksum { get; set; } = default!;
    public string FileLibraryPath { get; set; } = default!;
    public long Offset { get; set; }
    public int Size { get; set; }
    public Library Library { get; set; } = default!;
}