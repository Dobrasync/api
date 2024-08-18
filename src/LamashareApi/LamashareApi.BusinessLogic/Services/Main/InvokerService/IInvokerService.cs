using Lamashare.BusinessLogic.Services.Core.Jwt;
using LamashareApi.Database.DB.Entities;

namespace Lamashare.BusinessLogic.Services.Main.InvokerService;

public interface IInvokerService
{
    Task<User?> TryGetInvokerAsync();
    Task<User> GetInvokerAsyncThrows();
    AuthJwtClaims GetInvokerAuthJwtClaimsThrows();

}