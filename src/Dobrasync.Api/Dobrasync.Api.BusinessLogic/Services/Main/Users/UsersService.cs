using AutoMapper;
using Dobrasync.Api.BusinessLogic.Dtos.Library;
using Dobrasync.Api.BusinessLogic.Dtos.User;
using Dobrasync.Api.BusinessLogic.Util;
using Dobrasync.Api.Database.DB.Entities;
using Dobrasync.Api.Database.Repos;
using Gridify;
using Microsoft.EntityFrameworkCore;

namespace Dobrasync.Api.BusinessLogic.Services.Main.Users;

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