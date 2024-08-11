using AutoMapper;
using Gridify;
using Gridify.EntityFramework;
using Lamashare.BusinessLogic.Dtos.Library;
using Lamashare.BusinessLogic.Mapper.Gridify;
using LamashareApi.Database.Repos;
using Microsoft.EntityFrameworkCore;

namespace Lamashare.BusinessLogic.Services.Main.Library;

public class LibraryService(IRepoWrapper repoWrap, IMapper mapper) : ILibraryService
{
    public Task<Paging<LibraryDto>> GetAllLibrariesByUser(Guid userId, GridifyQuery gridifyQuery)
    {
        IQueryable<LamashareApi.Database.DB.Entities.Library> query = repoWrap
            .UserRepo
            .QueryAll()
            .Include(x => x.Libraries)
            .SelectMany(x => x.Libraries)
            .AsQueryable();

        return PaginateLibrary(query, gridifyQuery);
    }

    public Task<LibraryDto> CreateLibrary(LibraryCreateDto createDto)
    {
        throw new NotImplementedException();
    }

    public async Task<LibraryDto> GetLibraryById(Guid guid)
    {
        return mapper.Map<LibraryDto>(await repoWrap.LibraryRepo.GetByIdAsync(guid));
    }

    public Task<LibraryDto> UpdateLibrary(LibraryUpdateDto createDto)
    {
        throw new NotImplementedException();
    }

    public Task<LibraryDto> DeleteLibrary(Guid guid)
    {
        throw new NotImplementedException();
    }
    
    #region pagination

    private async Task<Paging<LibraryDto>> PaginateLibrary(IQueryable<LamashareApi.Database.DB.Entities.Library> queryable, GridifyQuery searchQuery)
    {
        Paging<LamashareApi.Database.DB.Entities.Library> filteredLibraries = await
            GetGridifyFilteredLibrariesAsync(queryable, searchQuery);
        List<LibraryDto> dtos = mapper.Map<List<LibraryDto>>(filteredLibraries);

        return new Paging<LibraryDto> { Data = dtos, Count = dtos.Count };
    }

    private async Task<Paging<LamashareApi.Database.DB.Entities.Library>> GetGridifyFilteredLibrariesAsync(IQueryable<LamashareApi.Database.DB.Entities.Library> queryable, GridifyQuery gridifyQuery)
    {
        LibraryGridifyMapper gridifyMapper = new();
        return await queryable.GridifyAsync(gridifyQuery, gridifyMapper);
    }
    #endregion
}