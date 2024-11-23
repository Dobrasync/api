using AutoMapper;
using Gridify;
using Lamashare.BusinessLogic.Dtos.Library;
using Lamashare.BusinessLogic.Dtos.User;
using Lamashare.BusinessLogic.Util;
using LamashareApi.Database.DB.Entities;
using LamashareApi.Database.Repos;
using LamashareApi.Shared.Exceptions.UserspaceException;
using Microsoft.EntityFrameworkCore;
using Zitadel.User.V1;

namespace Lamashare.BusinessLogic.Services.Main.Users;

public class UsersService(IRepoWrapper repoWrap, IMapper mapper) : IUsersService
{
    public async Task<UserDto> GetUserByIdAsync(Guid userId)
    {
        UserEntity user = await repoWrap.UserRepo.GetByIdAsyncThrows(userId);
        
        return mapper.Map<UserDto>(user);
    }
    
    public async Task<Paging<LibraryDto>> GetUserLibraries(Guid userId, GridifyQuery searchQuery)
    {
        UserEntity user = await repoWrap.UserRepo.GetByIdAsyncThrows(userId);

        return await repoWrap.UserRepo.QueryAll()
            .Include(x => x.Libraries)
            .Where(x => x.Id == user.Id)
            .SelectMany(x => x.Libraries)
            .PaginateAsync<LibraryEntity, LibraryDto>(searchQuery, mapper);
    }
}