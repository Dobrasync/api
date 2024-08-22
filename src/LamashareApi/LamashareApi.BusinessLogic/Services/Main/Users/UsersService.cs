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
        User newEntity = mapper.Map<User>(cdto);
        
        User inserted = await repoWrap.UserRepo.InsertAsync(newEntity);
        return mapper.Map<UserDto>(inserted);
    }
    
    public async Task<AuthDto> RegisterUser(RegisterUserDto rdto)
    {
        if (!await IsUsernameAvailable(rdto.Username))
            throw new UsernameTakenUSException(locs.GetLocKey(LocKeys.ExceptionUsernameTaken()));
        
        User newUser = new()
        {
            Username = rdto.Username,
            Password = SecretHasher.Hash(rdto.Password),
        };
        
        #region SUPERADMIN on first registration 
        // The first registered user should get superadmin rights
        if (await repoWrap.UserRepo.QueryAll().CountAsync() == 0)
        {
            newUser.Role = EUserRole.SUPERADMIN;
        }
        #endregion
        
        await repoWrap.UserRepo.InsertAsync(newUser);
        return authService.MakeAuthDto(AuthJwtClaims.FromUser(newUser));
    }

    public async Task<bool> IsUsernameAvailable(string username)
    {
        User? user = await repoWrap.UserRepo.QueryAll()
            .FirstOrDefaultAsync(x => x.Username.ToLower() == username.ToLower());

        return user == null;
    }

    public async Task<UserDto> GetUserByIdAsyncThrows(Guid guid)
    {
        return mapper.Map<UserDto>(await repoWrap.UserRepo.GetByIdAsync(guid));
    }
    
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

    public async Task<LibraryDto> CreateUserLibrary(Guid userId, LibraryCreateDto dto)
    {
        User user = await FindUserByIdAsyncThrows(userId);
        
        LamashareApi.Database.DB.Entities.Library library = mapper.Map<LamashareApi.Database.DB.Entities.Library>(dto);
        await repoWrap.LibraryRepo.InsertAsync(library);
        
        user.Libraries.Add(library);
        await repoWrap.UserRepo.UpdateAsync(user);
        
        return mapper.Map<LibraryDto>(library);
    }
    
    #region Util
    private async Task<User> FindUserByIdAsyncThrows(Guid id)
    {
        User? found = await repoWrap.UserRepo.GetByIdAsync(id);
        if (found == null) throw new NotFoundUSException("User not found");

        return found;
    }
    #endregion
    
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