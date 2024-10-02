using System.ComponentModel.DataAnnotations;

namespace LamashareApi.Database.DB.Entities;

public class File : BaseEntity
{
    public Guid Id { get; set; }
    public string TotalChecksum { get; set; } = default!;
    public Library Library { get; set; } = default!;
    [MaxLength(4096)] 
    public string FileLibraryPath { get; set; } = default!;

    public DateTime ModifiedOn { get; set; } = default!;
    public DateTime CreatedOn { get; set; } = default!;
    public List<FileTransaction> FileTransactions { get; set; } = new();
}