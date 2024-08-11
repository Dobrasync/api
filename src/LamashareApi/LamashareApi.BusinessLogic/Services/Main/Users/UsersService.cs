using AutoMapper;
using Lamashare.BusinessLogic.Dtos.User;
using LamashareApi.Database.Repos;

namespace Lamashare.BusinessLogic.Services.Main.Users;

public class UsersService(IMapper mapper, IRepoWrapper repoWrap) : IUsersService
{
    public async Task<UserDto> GetUserById(Guid guid)
    {
        return mapper.Map<UserDto>(await repoWrap.UserRepo.GetByIdAsync(guid));
    }
}