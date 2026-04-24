using System;
using System.Threading.Tasks;

namespace Pipelines.Generator.Test;

public sealed class PipelinesRegistrationTests : TestBase
{
    static readonly Action<TestContext> configure = context =>
    {
        context.IgnoreGeneratedResult = result => result.HintName == "PipelinesRegistrations.Handlers.g.cs";
    };

    [Test]
    public Task NoServiceCollectionAvailable()
    {
        return Verify(context =>
        {
            configure(context);
            context.References.RemoveAll(x => x.Display?.EndsWith("Microsoft.Extensions.DependencyInjection.dll") is true);
            context.Usings.RemoveAll(x => x == "Microsoft.Extensions.DependencyInjection");
        });
    }

    [Test]
    public Task Simple()
    {
        return Verify(configure);
    }
}
