using AutoMapper;
using Gridify;
using Gridify.EntityFramework;
using Lamashare.BusinessLogic.Dtos.Library;
using Lamashare.BusinessLogic.Mapper.Gridify;
using Lamashare.BusinessLogic.Services.Core.AppsettingsProvider;
using Lamashare.BusinessLogic.Services.Core.Localization;
using LamashareApi.Database.Repos;
using LamashareApi.Shared.Constants;
using LamashareApi.Shared.Exceptions.UserspaceException;
using LamashareApi.Shared.Localization;
using Microsoft.EntityFrameworkCore;

namespace Lamashare.BusinessLogic.Services.Main.Library;

public class LibraryService(IRepoWrapper repoWrap, IMapper mapper, ILocalizationService localizationService, IAppsettingsProvider apps) : ILibraryService
{

    public async Task<LibraryDto> CreateLibrary(LibraryCreateDto createDto)
    {
        LamashareApi.Database.DB.Entities.LibraryEntity? library = await repoWrap.LibraryRepo.QueryAll()
            .FirstOrDefaultAsync(x => x.Name.ToLower() == createDto.Name);

        if (library != null) throw new LibraryNameConflictUSException();
        
        LamashareApi.Database.DB.Entities.LibraryEntity inserted = await repoWrap.LibraryRepo.InsertAsync(new LamashareApi.Database.DB.Entities.LibraryEntity()
        {
            Name = createDto.Name
        });

        #region create new library dir
        string targetLibPath =
            LibraryUtil.GetLibraryDirectory(inserted.Id, apps.GetAppsettings().Storage.LibraryLocation);
        Directory.CreateDirectory(targetLibPath);
        #endregion
        
        return mapper.Map<LibraryDto>(inserted);
    }

    public async Task<LibraryDto> GetLibraryById(Guid guid)
    {
        LamashareApi.Database.DB.Entities.LibraryEntity? lib = await GetLibraryByIdShared(guid);

        return mapper.Map<LibraryDto>(lib);
    }

    public async Task<LibraryDto> UpdateLibrary(Guid id, LibraryUpdateDto dto)
    {
        LamashareApi.Database.DB.Entities.LibraryEntity lib = await GetLibraryByIdShared(id);

        mapper.Map(dto, lib);

        await repoWrap.LibraryRepo.UpdateAsync(lib);

        return mapper.Map<LibraryDto>(lib);
    }

    public Task<LibraryDto> DeleteLibrary(Guid guid)
    {
        throw new NotImplementedException();
    }
    
    #region get

    private async Task<LamashareApi.Database.DB.Entities.LibraryEntity> GetLibraryByIdShared(Guid id)
    {
        LamashareApi.Database.DB.Entities.LibraryEntity? lib = await repoWrap.LibraryRepo.QueryAll().FirstOrDefaultAsync(x => x.Id == id);

        if (lib == null)
            throw new NotFoundUSException();

        return lib;
    }
    #endregion
    
    #region pagination

    private async Task<Paging<LibraryDto>> PaginateLibrary(IQueryable<LamashareApi.Database.DB.Entities.LibraryEntity> queryable, GridifyQuery searchQuery)
    {
        Paging<LamashareApi.Database.DB.Entities.LibraryEntity> filteredLibraries = await
            GetGridifyFilteredLibrariesAsync(queryable, searchQuery);
        List<LibraryDto> dtos = mapper.Map<List<LibraryDto>>(filteredLibraries);

        return new Paging<LibraryDto> { Data = dtos, Count = dtos.Count };
    }

    private async Task<Paging<LamashareApi.Database.DB.Entities.LibraryEntity>> GetGridifyFilteredLibrariesAsync(IQueryable<LamashareApi.Database.DB.Entities.LibraryEntity> queryable, GridifyQuery gridifyQuery)
    {
        LibraryGridifyMapper gridifyMapper = new();
        return await queryable.GridifyAsync(gridifyQuery, gridifyMapper);
    }
    #endregion
}