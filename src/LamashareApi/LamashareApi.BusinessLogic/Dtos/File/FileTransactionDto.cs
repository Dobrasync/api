namespace Lamashare.BusinessLogic.Dtos.File;

public class FileTransactionDto
{
    public Guid Id { get; set; }
    public Guid FileId { get; set; }
    public EFileTransactionType Type { get; set; }
}