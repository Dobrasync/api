using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using LamashareApi.Database.Const;

namespace LamashareApi.Database.DB.Entities;

public class Block : BaseEntity
{
    public Guid Id { get; set; }
    
    [MinLength(LengthConstraints.BlockChecksumLength), MaxLength(LengthConstraints.BlockChecksumLength)]
    public string Checksum { get; set; } = default!;
    
    public File File { get; set; } = default!;
    
    public long Offset { get; set; }
    
    public int Size { get; set; }
    
    public Library Library { get; set; } = default!;
}