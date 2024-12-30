using Dobrasync.Api.Database.DB;

namespace Dobrasync.Api.Database.Init;

public static class DbInitializer
{
    public static async void InitializeAsync(DobrasyncContext context)
    {
        await context.Database.EnsureCreatedAsync();
        await context.SaveChangesAsync();
    }
}