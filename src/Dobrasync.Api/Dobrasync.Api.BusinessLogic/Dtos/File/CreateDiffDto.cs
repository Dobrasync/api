namespace Dobrasync.Api.BusinessLogic.Dtos.File;

public class CreateDiffDto
{
    public Guid LibraryId { get; set; }
    public List<FileInfoDto> FilesOnLocal { get; set; } = new();
}