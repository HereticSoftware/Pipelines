using System;
using System.Threading.Tasks;

namespace Pipelines.Generator.Test;

public sealed class HandlersRegistrationTests : TestBase
{
    static readonly Action<TestContext> configure = context =>
    {
        context.IgnoreGeneratedResult = result => result.HintName == "PipelinesRegistrations.Pipelines.g.cs";
    };

    [Test]
    public Task AbstractHandler()
    {
        return Verify(configure);
    }

    [Test]
    public Task DeepNamespace()
    {
        return Verify(configure);
    }

    [Test]
    public Task DuplicateHandlers()
    {
        // passes, todo: generate warnings about duplicate handler
        return Verify(configure);
    }

    [Test]
    public Task FileHandler()
    {
        return Verify(configure);
    }

    [Test]
    public Task GenericHandler()
    {
        return Verify(configure);
    }

    [Test]
    public Task InsideClass()
    {
        return Verify(configure);
    }

    [Test]
    public Task MultipleInterfaceImpl()
    {
        return Verify(configure);
    }

    [Test]
    public Task NoMessages()
    {
        return Verify(configure);
    }

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
    public Task PrivateHandler()
    {
        return Verify(configure);
    }

    [Test]
    public Task PrivateRequest()
    {
        return Verify(configure);
    }

    [Test]
    public Task PrivateResponse()
    {
        return Verify(configure);
    }

    [Test]
    public Task ProtectedHandler()
    {
        return Verify(configure);
    }

    [Test]
    public Task ProtectedRequest()
    {
        return Verify(configure);
    }

    [Test]
    public Task ProtectedResponse()
    {
        return Verify(configure);
    }

    [Test]
    public Task RequestWithContract()
    {
        return Verify(configure);
    }

    [Test]
    public Task RequestWithoutContract()
    {
        return Verify(configure);
    }

    [Test]
    public Task SimpleHandler()
    {
        return Verify(configure);
    }

    [Test]
    public Task StructHandler()
    {
        return Verify(configure);
    }

    [Test]
    public Task TwoHandlersDifferentNamespace()
    {
        return Verify(configure);
    }
}
