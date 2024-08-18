using AutoMapper;
using Lamashare.BusinessLogic.Dtos.Auth;
using Lamashare.BusinessLogic.Dtos.User;
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
            throw new UsernameTakenUSException(locs.GetLocKey(LocKeys.ExceptionUsernameTaken));
        
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

    public async Task<UserDto> GetUserById(Guid guid)
    {
        return mapper.Map<UserDto>(await repoWrap.UserRepo.GetByIdAsync(guid));
    }
}