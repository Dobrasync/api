using AutoMapper;
using Gridify;
using Gridify.EntityFramework;
using Lamashare.BusinessLogic.Dtos.Auth;
using Lamashare.BusinessLogic.Dtos.Library;
using Lamashare.BusinessLogic.Dtos.User;
using Lamashare.BusinessLogic.Mapper.Gridify;
using Lamashare.BusinessLogic.Services.Core.Jwt;
using Lamashare.BusinessLogic.Services.Core.Localization;
using Lamashare.BusinessLogic.Services.Main.Auth;
using Lamashare.BusinessLogic.Services.Main.UserRegistration;
using LamashareApi.Database.DB.Entities;
using LamashareApi.Database.Repos;
using LamashareApi.Shared.Exceptions.UserspaceException;
using LamashareApi.Shared.Localization;
using LamashareApi.Shared.Password;
using LamashareApi.Shared.Permissions;
using Microsoft.EntityFrameworkCore;

namespace Lamashare.BusinessLogic.Services.Main.Users;

public class UsersService(IMapper mapper, IRepoWrapper repoWrap, ILocalizationService locs, IAuthService authService) : IUsersService
{
    public async Task<UserDto> CreateUser(UserCreateDto cdto)
    {
        UserEntity newEntity = mapper.Map<UserEntity>(cdto);
        
        UserEntity inserted = await repoWrap.UserRepo.InsertAsync(newEntity);
        return mapper.Map<UserDto>(inserted);
    }
    
    public async Task<AuthDto> RegisterUser(RegisterUserDto rdto)
    {
        if (!await IsUsernameAvailable(rdto.Username))
            throw new UsernameTakenUSException();
        
        UserEntity newUserEntity = new()
        {
            Username = rdto.Username,
            Password = SecretHasher.Hash(rdto.Password),
        };
        
        #region SUPERADMIN on first registration 
        // The first registered user should get superadmin rights
        if (await repoWrap.UserRepo.QueryAll().CountAsync() == 0)
        {
            newUserEntity.Role = EUserRole.SUPERADMIN;
        }
        #endregion
        
        await repoWrap.UserRepo.InsertAsync(newUserEntity);
        return authService.MakeAuthDto(AuthJwtClaims.FromUser(newUserEntity));
    }

    public async Task<bool> IsUsernameAvailable(string username)
    {
        UserEntity? user = await repoWrap.UserRepo.QueryAll()
            .FirstOrDefaultAsync(x => x.Username.ToLower() == username.ToLower());

        return user == null;
    }

    public async Task<UserDto> GetUserByIdAsyncThrows(Guid guid)
    {
        return mapper.Map<UserDto>(await repoWrap.UserRepo.GetByIdAsync(guid));
    }
    
    public Task<Paging<LibraryDto>> GetAllLibrariesByUser(Guid userId, GridifyQuery gridifyQuery)
    {
        IQueryable<LamashareApi.Database.DB.Entities.LibraryEntity> query = repoWrap
            .UserRepo
            .QueryAll()
            .Include(x => x.Libraries)
            .SelectMany(x => x.Libraries)
            .AsQueryable();

        return PaginateLibrary(query, gridifyQuery);
    }

    public async Task<LibraryDto> CreateUserLibrary(Guid userId, LibraryCreateDto dto)
    {
        UserEntity userEntity = await FindUserByIdAsyncThrows(userId);
        
        LamashareApi.Database.DB.Entities.LibraryEntity libraryEntity = mapper.Map<LamashareApi.Database.DB.Entities.LibraryEntity>(dto);
        await repoWrap.LibraryRepo.InsertAsync(libraryEntity);
        
        userEntity.Libraries.Add(libraryEntity);
        await repoWrap.UserRepo.UpdateAsync(userEntity);
        
        return mapper.Map<LibraryDto>(libraryEntity);
    }
    
    #region Util
    private async Task<UserEntity> FindUserByIdAsyncThrows(Guid id)
    {
        UserEntity? found = await repoWrap.UserRepo.QueryAll().Include(x => x.Libraries).FirstOrDefaultAsync(x => x.Id == id);
        if (found == null) throw new NotFoundUSException();

        return found;
    }
    #endregion
    
    #region pagination

    private async Task<Paging<LibraryDto>> PaginateLibrary(IQueryable<LamashareApi.Database.DB.Entities.LibraryEntity> queryable, GridifyQuery searchQuery)
    {
        Paging<LamashareApi.Database.DB.Entities.LibraryEntity> filteredLibraries = await
            GetGridifyFilteredLibrariesAsync(queryable, searchQuery);
        List<LibraryDto> dtos = mapper.Map<List<LibraryDto>>(filteredLibraries.Data);

        return new Paging<LibraryDto> { Data = dtos, Count = dtos.Count };
    }

    private async Task<Paging<LamashareApi.Database.DB.Entities.LibraryEntity>> GetGridifyFilteredLibrariesAsync(IQueryable<LamashareApi.Database.DB.Entities.LibraryEntity> queryable, GridifyQuery gridifyQuery)
    {
        LibraryGridifyMapper gridifyMapper = new();
        return await queryable.GridifyAsync(gridifyQuery, gridifyMapper);
    }
    #endregion
}