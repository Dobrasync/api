using AutoMapper;
using Lamashare.BusinessLogic.Dtos.User;
using LamashareApi.Database.DB.Entities;
using LamashareApi.Database.Repos;
using LamashareApi.Shared.Permissions;
using Microsoft.EntityFrameworkCore;

namespace Lamashare.BusinessLogic.Services.Main.Users;

public class UsersService(IMapper mapper, IRepoWrapper repoWrap) : IUsersService
{
    public async Task<UserDto> CreateUser(UserCreateDto cdto)
    {
        User newEntity = mapper.Map<User>(cdto);
        
        #region SUPERADMIN on first registration 
        // The first registered user should get superadmin rights
        if (await repoWrap.UserRepo.QueryAll().CountAsync() == 0)
        {
            newEntity.Role = EUserRole.SUPERADMIN;
        }
        #endregion
        
        User inserted = await repoWrap.UserRepo.InsertAsync(newEntity);
        return mapper.Map<UserDto>(inserted);
    }

    public async Task<bool> IsUsernameAvailable(string username)
    {
        User? user = await repoWrap.UserRepo.QueryAll()
            .FirstOrDefaultAsync(x => x.Username.ToLower() == username.ToLower());

        return user == null;
    }

    public async Task<UserDto> GetUserById(Guid guid)
    {
        return mapper.Map<UserDto>(await repoWrap.UserRepo.GetByIdAsync(guid));
    }
}