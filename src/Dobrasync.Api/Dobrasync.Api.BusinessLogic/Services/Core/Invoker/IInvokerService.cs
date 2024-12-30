using Dobrasync.Api.Database.DB.Entities;

namespace Dobrasync.Api.BusinessLogic.Services.Core.Invoker;

public interface IInvokerService
{
    public Task<UserEntity> GetInvoker();
    public IQueryable<UserEntity> GetInvokerQuery();
}