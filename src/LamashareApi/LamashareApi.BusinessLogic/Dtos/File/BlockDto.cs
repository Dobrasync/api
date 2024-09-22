namespace Lamashare.BusinessLogic.Dtos.File;

public class BlockDto
{
    public string Checksum { get; set; } = default!;
    public byte[]? Content { get; set; }
}