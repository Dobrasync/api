using AutoMapper;
using Dobrasync.Api.BusinessLogic.Dtos.Library;
using Dobrasync.Api.BusinessLogic.Mapper.Gridify;
using Dobrasync.Api.BusinessLogic.Services.Core.AccessControl;
using Dobrasync.Api.BusinessLogic.Services.Core.AppsettingsProvider;
using Dobrasync.Api.BusinessLogic.Services.Core.Invoker;
using Dobrasync.Api.BusinessLogic.Services.Core.Localization;
using Dobrasync.Api.Database.DB.Entities;
using Dobrasync.Api.Database.Repos;
using Dobrasync.Api.Shared.Constants;
using Dobrasync.Api.Shared.Exceptions.UserspaceException;
using Gridify;
using Gridify.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Dobrasync.Api.BusinessLogic.Services.Main.Library;

public class LibraryService(
    IInvokerService invokerService,
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
        
        UserEntity invoker = await invokerService.GetInvokerQuery().Include(x => x.Libraries).FirstAsync();
        
        if (invoker.Libraries.Any(x => x.Name.ToLower() == createDto.Name)) throw new LibraryNameConflictUSException();

        LibraryEntity inserted = new()
        {
            Name = createDto.Name
        };
        invoker.Libraries.Add(inserted);
        await repoWrap.UserRepo.UpdateAsync(invoker);

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
        string directoryToDelete = LibraryUtil.GetLibraryDirectory(lib.Id, apps.GetAppsettings().Storage.LibraryLocation);
        
        #region Access-control
        (await acs.FromInvoker()).OwnsLibrary(lib);
        #endregion
        
        #region Delete library from DB
        await repoWrap.LibraryRepo.
            DeleteAsync(lib);
        #endregion
        #region delete library director and files
        Directory.Delete(directoryToDelete, true);
        #endregion
        
        return mapper.Map<LibraryDto>(lib);
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