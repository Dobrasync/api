using Dobrasync.Api.BusinessLogic.Dtos.Library;

namespace Dobrasync.Api.BusinessLogic.Services.Main.Library;

public interface ILibraryService
{
    Task<LibraryDto> CreateLibrary(LibraryCreateDto createDto);
    Task<LibraryDto> GetLibraryById(Guid guid);
    Task<LibraryDto> UpdateLibrary(Guid libraryGuid, LibraryUpdateDto createDto);
    Task<LibraryDto> DeleteLibrary(Guid guid);
}