using Lamashare.BusinessLogic.Dtos.File;
using Lamashare.BusinessLogic.Dtos.Library;
using Lamashare.BusinessLogic.Services.Main.File;
using Lamashare.BusinessLogic.Services.Main.Library;
using LamashareApi.Tests.Common;
using LamashareCore.Util;
using Microsoft.Extensions.DependencyInjection;

namespace LamashareApi.Tests.Fixtures;

public class GenericTestFixture : IAsyncLifetime
{
    public GenericTestFixture()
    {
        var services = new ServiceCollection();
        ServiceUtil.RegisterCommonServices(services);
        ServiceProvider = services.BuildServiceProvider();
    }

    public IServiceProvider ServiceProvider { get; }

    public async Task InitializeAsync()
    {
        var libraryService = ServiceProvider.GetRequiredService<ILibraryService>();
        var fileService = ServiceProvider.GetRequiredService<IFileService>();

        #region create lib

        var createdLibrary = await libraryService.CreateLibrary(new LibraryCreateDto
        {
            Name = LibraryName
        });
        LibraryId = createdLibrary.Id;

        #endregion

        #region upload a file

        #region create transaction

        var fileinfo = new FileInfo(TestFileSourcePath);
        var blocks = FileUtil.GetFileBlocks(TestFileSourcePath);
        var fileTotalChecksum = await FileUtil.GetFileTotalChecksumAsync(TestFileSourcePath);
        var transaction = await fileService.CreateFileTransaction(new FileTransactionCreateDto
        {
            LibraryId = createdLibrary.Id,
            Type = EFileTransactionType.PUSH,
            BlockChecksums = blocks.Select(x => x.Checksum).ToArray(),
            TotalChecksum = fileTotalChecksum,
            FileLibraryPath = TestFilePath,
            DateModifiedFile = fileinfo.LastWriteTimeUtc,
            DateCreatedFile = fileinfo.CreationTimeUtc
        });

        #endregion

        #region push blocks

        foreach (var block in blocks)
            await fileService.PushBlock(new BlockPushDto
            {
                TransactionId = transaction.Id,
                LibraryId = createdLibrary.Id,
                Checksum = block.Checksum,
                Content = block.Payload,
                Offset = block.Offset,
                Size = block.Payload.Length
            });

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

    #region public test data ref

    public static Guid LibraryId;
    public static readonly string LibraryName = "Test Library";

    public static readonly string TestFileSourcePath = "Resources/file.txt";
    public static readonly string TestFilePath = "my/nested/test/file.txt";
    public static readonly string TestFileContent = "TEST FILE CONTENT 123";

    #endregion
}