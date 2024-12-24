using Lamashare.BusinessLogic.Dtos.Auth;
using Lamashare.BusinessLogic.Dtos.Library;
using Lamashare.BusinessLogic.Services.Main.Auth;
using Lamashare.BusinessLogic.Services.Main.File;
using Lamashare.BusinessLogic.Services.Main.Library;
using LamashareApi.Database.DB.Entities;
using LamashareApi.Tests.Fixtures;
using Microsoft.Extensions.DependencyInjection;

namespace LamashareApi.Tests.Tests;

/// <summary>
/// Simulate normal user behaviour, multiple actions at once, etc. 
/// </summary>
[Collection("Sync")]
public class FlowTest
{
    private readonly IFileService fileService;
    private readonly IAuthService authService;
    private readonly ILibraryService libraryService;

    public FlowTest(NewInstanceTestFixture fixture)
    {
        fileService = fixture.ServiceProvider.GetRequiredService<IFileService>();
        libraryService = fixture.ServiceProvider.GetRequiredService<ILibraryService>();
        authService = fixture.ServiceProvider.GetRequiredService<IAuthService>();
    }
    
    [Fact]
    public async Task NewUserTest()
    {
        SessionInfoDto sessionInfo = await authService.GetSessionInfo();

        #region create library

        string initialLibraryName = "initial-library";
        LibraryDto initialLibrary = await CreateNewLibrary(initialLibraryName);

        #endregion
        
    }

    private async Task<LibraryDto> CreateNewLibrary(string name)
    {
        return await libraryService.CreateLibrary(new()
        {
            Name = name,
        });
    }
}