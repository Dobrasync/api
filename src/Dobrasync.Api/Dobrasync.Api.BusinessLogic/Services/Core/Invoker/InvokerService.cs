using System.IdentityModel.Tokens.Jwt;
using LamashareApi.Database.DB.Entities;
using LamashareApi.Database.Repos;
using LamashareApi.Shared.Exceptions.UserspaceException;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Zitadel.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zitadel.User.V1;

namespace Lamashare.BusinessLogic.Services.Core.Invoker;

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