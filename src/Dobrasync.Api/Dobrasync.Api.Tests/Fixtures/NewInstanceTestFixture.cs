using Dobrasync.Api.Tests.Common;
using Microsoft.Extensions.DependencyInjection;

namespace Dobrasync.Api.Tests.Fixtures;

public class NewInstanceTestFixture : IAsyncLifetime
{
    public IServiceProvider ServiceProvider { get; }
    
    public NewInstanceTestFixture()
    {
        var services = new ServiceCollection();
        ServiceUtil.RegisterCommonServices(services);
        ServiceProvider = services.BuildServiceProvider();
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}