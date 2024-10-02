namespace Lamashare.BusinessLogic.Dtos.File;

public class FileInfoDto
{
    public Guid FileId { get; set; }
    public Guid LibraryId { get; set; }
    public string TotalChecksum { get; set; } = default!;
    public string FileLibraryPath { get; set; } = default!;
    public DateTime ModifiedOn { get; set; }
    public DateTime CreatedOn { get; set; }
}