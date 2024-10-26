namespace Lamashare.BusinessLogic.Dtos.File;

public class FileInfoDto
{
    public Guid LibraryId { get; set; }
    public string TotalChecksum { get; set; } = default!;
    public string FileLibraryPath { get; set; } = default!;
    public DateTimeOffset DateModified { get; set; }
    public DateTimeOffset DateCreated { get; set; }
}