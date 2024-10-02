namespace Lamashare.BusinessLogic.Dtos.File;

public class FileTransactionCreateDto
{
    public Guid LibraryId { get; set; }
    public string FileLibraryPath { get; set; } = default!;
    public DateTime ModifiedOn { get; set; }
    public DateTime CreatedOn { get; set; }
    public EFileTransactionType Type { get; set; }
    
    /// <summary>
    /// List of block checksums - in desired final order.
    /// </summary>
    public string[] BlockChecksums { get; set; }
    
    /// <summary>
    /// The total checksum of the file after all blocks have been received
    /// and re-assembled.
    /// </summary>
    public string TotalChecksum { get; set; }
}