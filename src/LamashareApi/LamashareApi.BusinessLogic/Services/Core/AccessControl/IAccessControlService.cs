namespace Lamashare.BusinessLogic.Services.Core.AccessControl;

public interface IAccessControlService
{
    public Task<AccessControl> FromInvoker();
}