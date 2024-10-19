using System.ComponentModel.DataAnnotations;
using LamashareApi.Database.Const;
using LamashareApi.Database.DB.Entities.Base;

namespace LamashareApi.Database.DB.Entities;

public class BlockEntity : BaseEntity
{
    [MinLength(LengthConstraints.BlockChecksumLength)]
    [MaxLength(LengthConstraints.BlockChecksumLength)]
    public string Checksum { get; set; } = default!;

    public HashSet<FileEntity> Files { get; set; } = new();

    public long Offset { get; set; }

    public int Size { get; set; }

    public LibraryEntity Library { get; set; } = default!;
}