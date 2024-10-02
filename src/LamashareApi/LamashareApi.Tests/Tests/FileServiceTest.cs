using Lamashare.BusinessLogic.Dtos.File;
using Lamashare.BusinessLogic.Services.Main.File;
using Lamashare.BusinessLogic.Services.Main.Library;
using LamashareApi.Tests.Fixtures;
using LamashareCore.Util;
using Microsoft.Extensions.DependencyInjection;

namespace LamashareApi.Tests.Tests;

public class FileServiceTest : IClassFixture<GenericTestFixture>
{
    private readonly IFileService fileService;
    private readonly ILibraryService libraryService;

    public FileServiceTest(GenericTestFixture fixture)
    {
        fileService = fixture.ServiceProvider.GetRequiredService<IFileService>();
        libraryService = fixture.ServiceProvider.GetRequiredService<ILibraryService>();
    }
    
    [Fact]
    public async Task GetFileInfo()
    {
        var fileInfo = await fileService.GetFileInfo(GenericTestFixture.LibraryId, GenericTestFixture.TestFilePath);
        
        Assert.NotNull(fileInfo);
        Assert.Equal(GenericTestFixture.LibraryId, fileInfo.LibraryId);
        Assert.Equal(GenericTestFixture.TestFilePath, fileInfo.FileLibraryPath);
    }
    
    [Fact]
    public async Task GetFileTotalChecksum()
    {
        string expectedTotalChecksum = await FileUtil.GetFileTotalChecksumAsync(GenericTestFixture.TestFileSourcePath);
        var totalChecksum = await fileService.GetTotalChecksum(GenericTestFixture.LibraryId, GenericTestFixture.TestFilePath);
        
        Assert.Equal(expectedTotalChecksum, totalChecksum.Checksum);
    }
    
    [Fact]
    public async Task GetFileBlockList()
    {
        var expectedBlocklist = FileUtil.GetFileBlocks(GenericTestFixture.TestFileSourcePath).ToArray();
        var receivedBlocklist = await fileService.GetFileBlockList(GenericTestFixture.LibraryId, GenericTestFixture.TestFilePath);
        
        Assert.Equal(expectedBlocklist.Length, receivedBlocklist.Length);

        for (int i = 0; i < expectedBlocklist.Length; i++)
        {
            Assert.Equal(expectedBlocklist[i].Checksum, receivedBlocklist[i]);
        }
    }
    
    [Fact]
    public async Task PushFileBlockList()
    {
        #region create transaction
        string fileLibPath = "push-test/pushtest.txt";
        string sourcePath = "Resources/pushtest.txt";
        var blocklist = FileUtil.GetFileBlocks(sourcePath).ToArray();
        var totalchecksum = await FileUtil.GetFileTotalChecksumAsync(sourcePath);

        var transaction = await fileService.CreateFileTransaction(new()
        {
            FileLibraryPath = fileLibPath,
            BlockChecksums = blocklist.Select(x => x.Checksum).ToArray(),
            TotalChecksum = totalchecksum,
            ModifiedOn = DateTime.Now,
            CreatedOn = DateTime.Now,
            LibraryId = GenericTestFixture.LibraryId,
            Type = EFileTransactionType.PUSH
        });

        foreach (var block in blocklist)
        {
            await fileService.PushBlock(new()
            {
                Checksum = block.Checksum,
                Content = block.Payload,
                Offset = block.Offset,
                Size = block.Payload.Length,
                LibraryId = GenericTestFixture.LibraryId,
                TransactionId = transaction.Id,
                SourceFileLibraryPath = fileLibPath,
            });
        }

        var finalized = await fileService.FinishFileTransaction(transaction.Id);

        #endregion
    }
}