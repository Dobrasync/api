using Lamashare.BusinessLogic.Dtos.File;
using Lamashare.BusinessLogic.Dtos.Library;
using Lamashare.BusinessLogic.Services.Main.File;
using Lamashare.BusinessLogic.Services.Main.Library;
using LamashareApi.Tests.Common;
using Microsoft.Extensions.DependencyInjection;

namespace LamashareApi.Tests.Fixtures;

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