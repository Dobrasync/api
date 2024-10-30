using AutoMapper;
using Gridify;
using Gridify.EntityFramework;
using Lamashare.BusinessLogic.Dtos.Library;
using Lamashare.BusinessLogic.Mapper.Gridify;
using Lamashare.BusinessLogic.Services.Core.AccessControl;
using Lamashare.BusinessLogic.Services.Core.AppsettingsProvider;
using Lamashare.BusinessLogic.Services.Core.Localization;
using LamashareApi.Database.DB.Entities;
using LamashareApi.Database.Repos;
using LamashareApi.Shared.Constants;
using LamashareApi.Shared.Exceptions.UserspaceException;
using Microsoft.EntityFrameworkCore;

namespace Lamashare.BusinessLogic.Services.Main.Library;

public class LibraryService(
    IAccessControlService acs,
    IRepoWrapper repoWrap,
    IMapper mapper,
    ILocalizationService localizationService,
    IAppsettingsProvider apps) : ILibraryService
{
    #region POST - Create library
    public async Task<LibraryDto> CreateLibrary(LibraryCreateDto createDto)
    {
        #region Access-control
        await acs.FromInvoker();
        #endregion
        
        var library = await repoWrap.LibraryRepo.QueryAll()
            .FirstOrDefaultAsync(x => x.Name.ToLower() == createDto.Name);

        if (library != null) throw new LibraryNameConflictUSException();

        var inserted = await repoWrap.LibraryRepo.InsertAsync(new LibraryEntity
        {
            Name = createDto.Name
        });

        #region create new library dir

        var targetLibPath =
            LibraryUtil.GetLibraryDirectory(inserted.Id, apps.GetAppsettings().Storage.LibraryLocation);
        Directory.CreateDirectory(targetLibPath);

        #endregion

        return mapper.Map<LibraryDto>(inserted);
    }
    #endregion
    #region GET - Get library
    public async Task<LibraryDto> GetLibraryById(Guid guid)
    {
        var lib = await GetLibraryByIdShared(guid);
        
        #region Access-control
        (await acs.FromInvoker()).OwnsLibrary(lib);
        #endregion

        return mapper.Map<LibraryDto>(lib);
    }
    #endregion
    #region PUT - Update library
    public async Task<LibraryDto> UpdateLibrary(Guid id, LibraryUpdateDto dto)
    {
        var lib = await GetLibraryByIdShared(id);
        
        #region Access-control
        (await acs.FromInvoker()).OwnsLibrary(lib);
        #endregion

        mapper.Map(dto, lib);

        await repoWrap.LibraryRepo.UpdateAsync(lib);

        return mapper.Map<LibraryDto>(lib);
    }
    #endregion
    #region DELETE - Delete library
    public async Task<LibraryDto> DeleteLibrary(Guid guid)
    {
        var lib = await GetLibraryByIdShared(guid);
        
        #region Access-control
        (await acs.FromInvoker()).OwnsLibrary(lib);
        #endregion
        
        throw new NotImplementedException();
    }
    #endregion

    #region common
    private async Task<LibraryEntity> GetLibraryByIdShared(Guid id)
    {
        var lib = await repoWrap.LibraryRepo.QueryAll().FirstOrDefaultAsync(x => x.Id == id);

        if (lib == null)
            throw new NotFoundUSException();

        return lib;
    }
    #endregion
    #region pagination

    private async Task<Paging<LibraryDto>> PaginateLibrary(IQueryable<LibraryEntity> queryable,
        GridifyQuery searchQuery)
    {
        var filteredLibraries = await
            GetGridifyFilteredLibrariesAsync(queryable, searchQuery);
        var dtos = mapper.Map<List<LibraryDto>>(filteredLibraries);

        return new Paging<LibraryDto> { Data = dtos, Count = dtos.Count };
    }

    private async Task<Paging<LibraryEntity>> GetGridifyFilteredLibrariesAsync(IQueryable<LibraryEntity> queryable,
        GridifyQuery gridifyQuery)
    {
        LibraryGridifyMapper gridifyMapper = new();
        return await queryable.GridifyAsync(gridifyQuery, gridifyMapper);
    }

    #endregion
}