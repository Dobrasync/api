using Dobrasync.Api.BusinessLogic.Services.Core.Invoker;
using Dobrasync.Api.Database.DB.Entities;
using Dobrasync.Api.Database.Repos;
using Microsoft.EntityFrameworkCore;

namespace Dobrasync.Api.BusinessLogic.Services.Core.AccessControl;

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