using System.Linq.Expressions;
using Dobrasync.Api.Shared.Exceptions.UserspaceException;
using Microsoft.EntityFrameworkCore;

namespace Dobrasync.Api.Database.Extensions;

public static class QueryableExtensions
{
    public static async Task<T> FirstOrThrowAsync<T>(
        this IQueryable<T> source)
    {
        var result = await source.FirstOrDefaultAsync();

        if (result == null)
        {
            throw new NotFoundUSException();
        }

        return result;
    }
    
    public static async Task<T> FirstOrThrowAsync<T>(
        this IQueryable<T> source,
        Expression<Func<T, bool>>? predicate)
    {
        T? result;
        if (predicate == null)
        {
            result = await source.FirstOrDefaultAsync();
        }
        else
        {
            result = await source.FirstOrDefaultAsync(predicate);
        }

        if (result == null)
        {
            throw new NotFoundUSException();
        }

        return result;
    }
}