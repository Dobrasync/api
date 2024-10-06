using Lamashare.BusinessLogic.Services.Main.File;
using Lamashare.BusinessLogic.Services.Main.Library;
using LamashareApi.Tests.Fixtures;
using LamashareCore.Util;
using Microsoft.Extensions.DependencyInjection;

namespace LamashareApi.Tests.Tests;

[Collection("Sync")]
public class DiffTests : IClassFixture<GenericTestFixture>
{
    private readonly IFileService fileService;
    private readonly ILibraryService libraryService;

    public DiffTests(GenericTestFixture fixture)
    {
        fileService = fixture.ServiceProvider.GetRequiredService<IFileService>();
        libraryService = fixture.ServiceProvider.GetRequiredService<ILibraryService>();
    }
    
    [Fact]
    public async Task GetFileDiff()
    {
        string localFilePath = "Resources/pushtest.txt";
        var localFileInfo = new FileInfo(localFilePath);
        var localTotalChecksum = await FileUtil.GetFileTotalChecksumAsync(localFilePath);
        var diff = await fileService.CreateLibraryDiff(new()
        {
            LibraryId = GenericTestFixture.LibraryId,
            FilesOnLocal = [new()
            {
                LibraryId = GenericTestFixture.LibraryId,
                DateCreated = localFileInfo.CreationTimeUtc,
                DateModified = localFileInfo.LastWriteTimeUtc,
                TotalChecksum = localTotalChecksum,
                FileLibraryPath = localFilePath,
            }],
        });
        
        Assert.Single(diff.RequiredByLocal);
        Assert.Single(diff.RequiredByRemote);
    }
    
    [Fact]
    public async Task GetFileDiffNoLocalFiles()
    {
        var diff = await fileService.CreateLibraryDiff(new()
        {
            LibraryId = GenericTestFixture.LibraryId,
            FilesOnLocal = [],
        });
        
        Assert.Empty(diff.RequiredByRemote);
        Assert.Single(diff.RequiredByLocal);
        Assert.NotNull(diff.RequiredByLocal.FirstOrDefault(x => x == GenericTestFixture.TestFilePath));
    }

}