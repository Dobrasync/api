using Dobrasync.Api.BusinessLogic.Dtos.File;
using Dobrasync.Api.BusinessLogic.Services.Main.File;
using Dobrasync.Api.BusinessLogic.Services.Main.Library;
using Dobrasync.Api.Database.Enums;
using Dobrasync.Api.Tests.Fixtures;
using Dobrasync.Core.Common.Util;
using Microsoft.Extensions.DependencyInjection;

namespace Dobrasync.Api.Tests.Tests;

[Collection("Sync")]
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
        var expectedTotalChecksum = await FileUtil.GetFileTotalChecksumAsync(GenericTestFixture.TestFileSourcePath);
        var totalChecksum =
            await fileService.GetTotalChecksum(GenericTestFixture.LibraryId, GenericTestFixture.TestFilePath);

        Assert.Equal(expectedTotalChecksum, totalChecksum.Checksum);
    }

    [Fact]
    public async Task GetFileBlockList()
    {
        var expectedBlocklist = FileUtil.GetFileBlocks(GenericTestFixture.TestFileSourcePath).ToArray();
        var receivedBlocklist =
            await fileService.GetFileBlockList(GenericTestFixture.LibraryId, GenericTestFixture.TestFilePath);

        Assert.Equal(expectedBlocklist.Length, receivedBlocklist.Length);

        for (var i = 0; i < expectedBlocklist.Length; i++)
            Assert.Equal(expectedBlocklist[i].Checksum, receivedBlocklist[i]);
    }

    [Fact]
    public async Task PushFileBlockList()
    {
        #region create transaction

        var fileLibPath = "push-test/pushtest.txt";
        var sourcePath = "Resources/pushtest.txt";
        var blocklist = FileUtil.GetFileBlocks(sourcePath).ToArray();
        var totalchecksum = await FileUtil.GetFileTotalChecksumAsync(sourcePath);

        var transaction = await fileService.CreateFileTransaction(new FileTransactionCreateDto
        {
            FileLibraryPath = fileLibPath,
            BlockChecksums = blocklist.Select(x => x.Checksum).ToArray(),
            TotalChecksum = totalchecksum,
            DateModifiedFile = DateTime.Now,
            DateCreatedFile = DateTime.Now,
            LibraryId = GenericTestFixture.LibraryId,
            Type = EFileTransactionType.PUSH
        });

        foreach (var block in blocklist)
            await fileService.PushBlock(new BlockPushDto
            {
                LibraryId = GenericTestFixture.LibraryId,
                TransactionId = transaction.Id,

                Checksum = block.Checksum,
                Content = block.Payload,
                Offset = block.Offset,
                Size = block.Payload.Length
            });

        var finalized = await fileService.FinishFileTransaction(transaction.Id);

        #endregion
    }

    [Fact]
    public async Task PullFileBlockList()
    {
        #region get file info

        var fileInfo = await fileService.GetFileInfo(GenericTestFixture.LibraryId, GenericTestFixture.TestFilePath);

        #endregion

        #region create transaction

        var trans = await fileService.CreateFileTransaction(new FileTransactionCreateDto
        {
            FileLibraryPath = GenericTestFixture.TestFilePath,
            LibraryId = GenericTestFixture.LibraryId,
            Type = EFileTransactionType.PULL
        });

        #endregion

        #region pull blocks

        var blocklist =
            await fileService.GetFileBlockList(GenericTestFixture.LibraryId, GenericTestFixture.TestFilePath);
        var receivedBlocks = new List<BlockDto>();
        foreach (var block in blocklist)
        {
            var received = await fileService.PullBlock(block);
            receivedBlocks.Add(new BlockDto
            {
                Checksum = received.Checksum,
                Content = received.Content
            });
        }

        #endregion

        #region finish transaction

        var finishedTrans = await fileService.FinishFileTransaction(trans.Id);

        #endregion

        #region get temp file

        var target = Path.GetTempFileName();
        var bytes = receivedBlocks.SelectMany(x => x.Content!).ToArray();
        await File.WriteAllBytesAsync(target, bytes);
        var fileTarget = Path.GetTempFileName();
        await FileUtil.FullRestoreFileFromBlocks([target], fileTarget, fileInfo.DateCreated, fileInfo.DateModified);

        #endregion

        var expectedCheck = await FileUtil.GetFileTotalChecksumAsync(GenericTestFixture.TestFileSourcePath);
        var receivedCheck = await FileUtil.GetFileTotalChecksumAsync(fileTarget);
        Assert.Equal(expectedCheck, receivedCheck);
    }
}