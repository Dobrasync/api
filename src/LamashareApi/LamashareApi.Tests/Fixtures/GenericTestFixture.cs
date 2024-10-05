using Lamashare.BusinessLogic.Dtos.File;
using Lamashare.BusinessLogic.Services.Main.File;
using Lamashare.BusinessLogic.Services.Main.Library;
using LamashareApi.Database.DB;
using LamashareApi.Database.Repos;
using LamashareApi.Tests.Common;
using LamashareCore.Util;
using Microsoft.Extensions.DependencyInjection;

namespace LamashareApi.Tests.Fixtures;

public class GenericTestFixture : IAsyncLifetime
{
    #region public test data ref
    public static Guid LibraryId = new Guid();
    public static readonly string LibraryName = "Test Library";
    
    public static readonly string TestFileSourcePath = "Resources/file.txt";
    public static readonly string TestFilePath = "my/nested/test/file.txt";
    public static readonly string TestFileContent = "TEST FILE CONTENT 123";
    #endregion
    
    public IServiceProvider ServiceProvider { get; private set; }
    
    public GenericTestFixture()
    {
        var services = new ServiceCollection();
        ServiceUtil.RegisterCommonServices(services);
        ServiceProvider = services.BuildServiceProvider();
    }

    public async Task InitializeAsync()
    {
        var libraryService = ServiceProvider.GetRequiredService<ILibraryService>();
        var fileService = ServiceProvider.GetRequiredService<IFileService>();

        #region create lib
        var createdLibrary = await libraryService.CreateLibrary(new()
        {
            Name = LibraryName
        });
        LibraryId = createdLibrary.Id;
        #endregion
        #region upload a file
        #region create transaction
        var fileinfo = new FileInfo(TestFileSourcePath);
        var blocks = FileUtil.GetFileBlocks(TestFileSourcePath);
        string fileTotalChecksum = await FileUtil.GetFileTotalChecksumAsync(TestFileSourcePath);
        var transaction = await fileService.CreateFileTransaction(new()
        {
            LibraryId = createdLibrary.Id,
            Type = EFileTransactionType.PUSH,
            BlockChecksums = blocks.Select(x => x.Checksum).ToArray(),
            TotalChecksum = fileTotalChecksum,
            FileLibraryPath = TestFilePath,
            ModifiedOn = fileinfo.LastWriteTimeUtc,
            CreatedOn = fileinfo.CreationTimeUtc,
        });
        #endregion
        #region push blocks
        foreach (var block in blocks)
        {
            await fileService.PushBlock(new()
            {
                TransactionId = transaction.Id,
                LibraryId = createdLibrary.Id,
                FileId = transaction.FileId,
                Checksum = block.Checksum,
                Content = block.Payload,
                Offset = block.Offset,
                Size = block.Payload.Length,
            });
        }
        #endregion
        #region finalize transaction
        var finalizationResult = await fileService.FinishFileTransaction(transaction.Id);
        #endregion
        #endregion
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}