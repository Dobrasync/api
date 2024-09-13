namespace LamashareCore.Dto;

public class FileBlockDto
{
    public Guid Id { get; set; }
    public string Checksum { get; set; }
    public byte[] Bytes { get; set; }
}