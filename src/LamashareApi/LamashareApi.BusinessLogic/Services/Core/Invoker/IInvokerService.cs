using LamashareApi.Database.DB.Entities;

namespace Lamashare.BusinessLogic.Services.Core.Invoker;

public interface IInvokerService
{
    public Task<UserEntity> GetInvoker();
    public IQueryable<UserEntity> GetInvokerQuery();
}