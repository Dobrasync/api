using System.ComponentModel.DataAnnotations;
using LamashareApi.Database.Const;

namespace LamashareApi.Database.DB.Entities;

public class FileEntity : BaseEntity
{
    [MinLength(LengthConstraints.TotalChecksumLength), MaxLength(LengthConstraints.TotalChecksumLength)] 
    public string TotalChecksum { get; set; } = default!;
    
    public LibraryEntity Library { get; set; } = default!;
    
    [MinLength(LengthConstraints.FileLibraryPathMinLength), MaxLength(LengthConstraints.FileLibraryPathMaxLength)] 
    public string FileLibraryPath { get; set; } = default!;
    
    public DateTimeOffset DateModified { get; set; }
    
    public DateTimeOffset DateCreated { get; set; }
    
    public List<FileTransactionEntity> FileTransactions { get; set; } = new();
    
    public List<BlockEntity> Blocks { get; set; } = new();
}