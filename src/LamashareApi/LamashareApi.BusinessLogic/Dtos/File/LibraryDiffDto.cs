namespace Lamashare.BusinessLogic.Dtos.File;

public class LibraryDiffDto
{
    public List<string> RequiredByRemote { get; set; }
    public List<string> RequiredByLocal { get; set; }
}