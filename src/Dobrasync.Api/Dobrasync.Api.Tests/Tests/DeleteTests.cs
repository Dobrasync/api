using Dobrasync.Api.BusinessLogic.Services.Core.AppsettingsProvider;
using Dobrasync.Api.BusinessLogic.Services.Main.File;
using Dobrasync.Api.BusinessLogic.Services.Main.Library;
using Dobrasync.Api.Database.Repos;
using Dobrasync.Api.Shared.Constants;
using Dobrasync.Api.Tests.Fixtures;
using Dobrasync.Core.Common.Util;
using Microsoft.Extensions.DependencyInjection;

namespace Dobrasync.Api.Tests.Tests;

[Collection("Sync")]
public class DeleteTests : IClassFixture<GenericTestFixture>
{
    private readonly IAppsettingsProvider apps;
    private readonly IFileService fileService;
    private readonly ILibraryService libraryService;
    private readonly IRepoWrapper repoWrap;

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

        var libPath = LibraryUtil.GetLibraryDirectory(GenericTestFixture.LibraryId,
            apps.GetAppsettings().Storage.LibraryLocation);

        var fileStillExistsInFileSystem =
            File.Exists(FileUtil.FileLibPathToSysPath(libPath, GenericTestFixture.TestFilePath));

        Assert.False(fileStillExistsInFileSystem);
        Assert.Equal(0, repoWrap.BlockRepo.QueryAll().Count());
    }
}