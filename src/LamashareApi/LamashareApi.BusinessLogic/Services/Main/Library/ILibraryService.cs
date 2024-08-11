using Gridify;
using Lamashare.BusinessLogic.Dtos.Library;

namespace Lamashare.BusinessLogic.Services.Main.Library;

public interface ILibraryService
{
    Task<Paging<LibraryDto>> GetAllLibrariesByUser(Guid userId, GridifyQuery query);
    Task<LibraryDto> CreateLibrary(LibraryCreateDto createDto);
    Task<LibraryDto> GetLibraryById(Guid guid);
    Task<LibraryDto> UpdateLibrary(LibraryUpdateDto createDto);
    Task<LibraryDto> DeleteLibrary(Guid guid);
}