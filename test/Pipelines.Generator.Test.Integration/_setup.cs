global using Microsoft.Extensions.DependencyInjection;
global using Pipelines.Generator.Test.Integration.Helpers;
global using static TUnit.Assertions.Assert;

namespace Pipelines.Generator.Test.Integration;

public abstract class TestBase
{
    /// <summary>
    /// Setups a basic service collection, will register services using the following order:
    /// <list type="number">
    /// <item>setupAction</item>
    /// <item>AddHandlers</item>
    /// <item>AddPipelines</item>
    /// <item>AddScoped{RequestContext}</item>
    /// </list>
    /// </summary>
    protected static IServiceProvider SetupServices(Action<IServiceCollection>? setupAction = null)
    {
        var services = new ServiceCollection();

        setupAction?.Invoke(services);
        services.AddHandlers();
        services.AddPipelines();
        services.AddScoped<RequestContext>();

        return services.BuildServiceProvider();
    }
}
