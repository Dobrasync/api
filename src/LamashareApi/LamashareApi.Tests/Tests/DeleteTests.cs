using Lamashare.BusinessLogic.Services.Core.AppsettingsProvider;
using Lamashare.BusinessLogic.Services.Main.File;
using Lamashare.BusinessLogic.Services.Main.Library;
using LamashareApi.Database.Repos;
using LamashareApi.Shared.Constants;
using LamashareApi.Tests.Fixtures;
using LamashareCore.Util;
using Microsoft.Extensions.DependencyInjection;

namespace LamashareApi.Tests.Tests;

[Collection("Sync")]
public class DeleteTests : IClassFixture<GenericTestFixture>
{
    private readonly IFileService fileService;
    private readonly ILibraryService libraryService;
    private readonly IRepoWrapper repoWrap;
    private readonly IAppsettingsProvider apps;

    public DeleteTests(GenericTestFixture fixture)
    {
        fileService = fixture.ServiceProvider.GetRequiredService<IFileService>();
        libraryService = fixture.ServiceProvider.GetRequiredService<ILibraryService>();
        repoWrap = fixture.ServiceProvider.GetRequiredService<IRepoWrapper>();
        apps = fixture.ServiceProvider.GetRequiredService<IAppsettingsProvider>();
    }
    
    [Fact]
    public async Task DeleteFile()
    {
        await fileService.DeleteFile(GenericTestFixture.LibraryId, GenericTestFixture.TestFilePath);

        string libPath = LibraryUtil.GetLibraryDirectory(GenericTestFixture.LibraryId,
            apps.GetAppsettings().Storage.LibraryLocation);

        bool fileStillExistsInFileSystem =
            File.Exists(FileUtil.FileLibPathToSysPath(libPath, GenericTestFixture.TestFilePath));
        
        Assert.False(fileStillExistsInFileSystem);
        Assert.Equal(0, repoWrap.BlockRepo.QueryAll().Count()); 
    }

}