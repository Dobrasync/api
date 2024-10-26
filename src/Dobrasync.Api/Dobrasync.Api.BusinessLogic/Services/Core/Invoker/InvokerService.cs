using System.IdentityModel.Tokens.Jwt;
using LamashareApi.Database.DB.Entities;
using LamashareApi.Database.Repos;
using LamashareApi.Shared.Exceptions.UserspaceException;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Lamashare.BusinessLogic.Services.Core.Invoker;

public class InvokerService(IRepoWrapper repoWrap, IHttpContextAccessor contextAccessor) : IInvokerService
{
    public async Task<UserEntity> GetInvoker()
    {
        UserEntity? user = await GetInvokerQuery().FirstOrDefaultAsync();
        if (user == null)
        {
            throw new InvalidAuthTokenUSException();
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

        if (context?.Request.Headers.TryGetValue("Authorization", out var authHeader) == true)
        {
            // Ensure the header starts with "Bearer "
            if (authHeader.ToString().StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                var token = authHeader.ToString().Substring("Bearer ".Length).Trim();

                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                // Check if the 'preferred_username' claim exists
                var preferredUsernameClaim = jwtToken.Claims
                    .FirstOrDefault(c => c.Type == "preferred_username");

                return preferredUsernameClaim?.Value;
            }
        }

        return null; // or throw an exception if preferred
    }
}