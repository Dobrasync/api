using System.ComponentModel.DataAnnotations;
using LamashareApi.Database.Const;

namespace LamashareApi.Database.DB.Entities;

public class File : BaseEntity
{
    public Guid Id { get; set; }
    
    [MinLength(LengthConstraints.TotalChecksumLength), MaxLength(LengthConstraints.TotalChecksumLength)] 
    public string TotalChecksum { get; set; } = default!;
    
    public Library Library { get; set; } = default!;
    
    [MinLength(LengthConstraints.FileLibraryPathMinLength), MaxLength(LengthConstraints.FileLibraryPathMaxLength)] 
    public string FileLibraryPath { get; set; } = default!;
    
    public DateTime ModifiedOn { get; set; } = default!;
    
    public DateTime CreatedOn { get; set; } = default!;
    
    public List<FileTransaction> FileTransactions { get; set; } = new();
    
    public List<Block> Blocks { get; set; } = new();
}