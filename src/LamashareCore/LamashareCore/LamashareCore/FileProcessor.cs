using System.Security.Cryptography;
using System.Text;

namespace LamashareCore;

public class FileProcessor : IFileProcessor
{
    public IEnumerable<byte[]> GenerateBlocksFromFile(string filePath)
    {
        int blockSize = 1024;
        
        using FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        byte[] buffer = new byte[blockSize];
        
        int bytesRead;
        while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
        {
            // If the last block is smaller than the buffer size, 
            // copy it to a smaller array before yielding.
            if (bytesRead < blockSize)
            {
                byte[] lastBuffer = new byte[bytesRead];
                Array.Copy(buffer, lastBuffer, bytesRead);
                yield return lastBuffer;
            }
            else
            {
                yield return buffer;
            }

            // Reset the buffer if the file size is a multiple of blockSize to avoid returning a stale block
            if (bytesRead == blockSize)
            {
                buffer = new byte[blockSize];
            }
        }
    }

    public string GenerateChecksumFromBlock(byte[] block)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(block);
            
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
}