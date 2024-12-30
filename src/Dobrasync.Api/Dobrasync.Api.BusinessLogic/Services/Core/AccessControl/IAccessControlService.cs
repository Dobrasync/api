namespace Dobrasync.Api.BusinessLogic.Services.Core.AccessControl;

public interface IAccessControlService
{
    public Task<AccessControl> FromInvoker();
}