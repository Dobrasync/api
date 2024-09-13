namespace LamashareCore;

public interface IFileProcessor
{
    public string GenerateChecksumFromBlock(byte[] block);
    public IEnumerable<byte[]> GenerateBlocksFromFile(string filePath);
}