using Dobrasync.Api.Database.DB.Entities;
using Dobrasync.Api.Database.Repos;
using Dobrasync.Api.Shared.Exceptions.UserspaceException;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Dobrasync.Api.BusinessLogic.Services.Core.Invoker;

public class InvokerService(IRepoWrapper repoWrap, IHttpContextAccessor contextAccessor) : IInvokerService
{
    public async Task<UserEntity> GetInvoker()
    {
        UserEntity? user = await GetInvokerQuery().FirstOrDefaultAsync();
        if (user == null)
        {
            
            // Create new user if not exists
            string? username = GetPreferredUsername();
            if (username == null) throw new NotFoundUSException();
            
            UserEntity newUser = new()
            {
                Username = username
            };
            
            await repoWrap.UserRepo.InsertAsync(newUser);
            user = newUser;
        }

        return user;
    }
    
    public IQueryable<UserEntity> GetInvokerQuery()
    {
        return repoWrap.UserRepo.QueryAll().Where(x => x.Username == GetPreferredUsername());
    }
    
    private string? GetPreferredUsername()
    {
        var context = contextAccessor.HttpContext;

        return context?.User?.Claims.FirstOrDefault(x => x.Type == "preferred_username")?.Value;
    }
}