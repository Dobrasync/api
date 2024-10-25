using Lamashare.BusinessLogic.Services.Core.Invoker;
using LamashareApi.Database.DB.Entities;
using LamashareApi.Shared.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Lamashare.BusinessLogic.Services.Core.AccessControl;

public class AccessControlService(IInvokerService invokerService) : IAccessControlService
{
    public async Task<AccessControl> FromInvoker()
    {
        UserEntity user = await invokerService.GetInvokerQuery()
            .Include(x => x.Libraries)
            .FirstOrThrowAsync();
        return new AccessControl()
        {
            Invoker = user
        };
    }
}