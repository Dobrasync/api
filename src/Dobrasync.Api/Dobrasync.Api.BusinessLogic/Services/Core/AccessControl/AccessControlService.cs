using Lamashare.BusinessLogic.Services.Core.Invoker;
using LamashareApi.Database.DB.Entities;
using LamashareApi.Database.Repos;
using LamashareApi.Shared.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Lamashare.BusinessLogic.Services.Core.AccessControl;

public class AccessControlService(IInvokerService invokerService, IRepoWrapper repoWrap) : IAccessControlService
{
    public async Task<AccessControl> FromInvoker()
    {
        UserEntity user = await invokerService.GetInvoker();

        UserEntity fullyIncludedUser = await repoWrap.UserRepo.QueryAll()
            .Include(x => x.Libraries)
            .FirstAsync(x => x.Id == user.Id);
                          
        return new AccessControl()
        {
            Invoker = fullyIncludedUser,
        };
    }
}