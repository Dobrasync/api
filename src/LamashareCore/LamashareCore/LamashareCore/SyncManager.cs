using System.Reflection.Metadata;
using System.Text.Json;
using LamashareCore.Dto;
using LamashareCore.Etc;
using LamashareCore.Models;
using Microsoft.VisualBasic;

namespace LamashareCore;

public class SyncManager
{
    public void SyncFile(Guid libraryId, string libraryFilePath)
    {
        FileProcessor fp = new();

        var localBlocks = fp.GenerateBlocksFromFile(libraryFilePath);
        
        #region Fetch remote block list and compare to local
        
        // First we fetch the remote block list and then compare it with
        // the list we generated locally. We then submit only the
        // blocks that are not found in the remote. Order obvsly. matters.
        var remoteBlocks = GetFileRemoteBlocklist(libraryId, libraryFilePath);
        
        
        
        #region Submit changed blocks
        
        #endregion
        
        #endregion

        
    }

    private async Task<List<FileBlockDto>> GetFileRemoteBlocklist(Guid libraryId, string libraryFilePath)
    {
        using HttpClient client = new HttpClient();
        try
        {
            string url = CoreConstants.GetApiUrlFileBlocklist(libraryId);
            var content = new StringContent(JsonSerializer.Serialize(new GetFileBlocklistDto()
            {
                Filepath = libraryFilePath,
            })); 
            
            HttpResponseMessage response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            
            // Response should look like [{id, checksum},...]
            string responseBody = await response.Content.ReadAsStringAsync();
            var dataItems = JsonSerializer.Deserialize<List<FileBlockDto>>(responseBody);

            return dataItems;
        }
        catch (HttpRequestException e)
        {
            // Handle any exceptions that may occur
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message :{0} ", e.Message);
        }
    }

    private EBlockSyncStatus CompareBlock(byte[] localBlock, string file)
    {
        return EBlockSyncStatus.NEWER_LOCAL;
        return EBlockSyncStatus.NEWER_REMOTE;
        
        return EBlockSyncStatus.EQUAL;
    }

    private string FetchFileFromRemote(string filepath)
    {
        // Fetch only block
    }

    private string SubmitFileToRemote(string filepath)
    {
        // Submit only block
    }
}