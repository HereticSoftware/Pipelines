using Pipelines.Attributes;
using Pipelines.Requests;

namespace Pipelines.Generator.Test.Integration;

public sealed class IgnoreAttributeTests : TestBase
{
    [Test]
    public async Task Should_Not_Register_Handler()
    {
        var services = SetupServices();

        var pingHandler = services.GetService<IRequestHandler<Ping, Pong>>();
        await That(pingHandler).IsNull();

        var pongHandler = services.GetService<IRequestHandler<Pong, Ping>>();
        await That(pongHandler).IsNull();
    }

    [Test]
    public async Task Should_Not_Resolve_Pipeline()
    {
        var services = SetupServices();

        var pingPipline = () => services.GetService<RequestPipeline<Ping, Pong>>();
        await That(pingPipline).Throws<InvalidOperationException>().WithMessageContaining("Unable to resolve service");

        var pongPipline = () => services.GetService<RequestPipeline<Pong, Ping>>();
        await That(pongPipline).Throws<InvalidOperationException>().WithMessageContaining("Unable to resolve service"); ;
    }

    public sealed class Ping : IRequest<Ping, Pong>;

    public sealed record Pong(int Num) : IRequest<Pong, Ping>;

    [Ignore]
    public sealed class PingHandler(RequestContext context) : IRequestHandler<Ping, Pong>, IRequestHandler<Pong, Ping>
    {
        public ValueTask<Pong> Handle(Ping request, CancellationToken cancellationToken = default)
        {
            context.CallStack.Push(GetType().Name);
            return new(new Pong(100));
        }

        public ValueTask<Ping> Handle(Pong request, CancellationToken cancellationToken = default)
        {
            context.CallStack.Push(GetType().Name);
            return new(new Ping());
        }
    }
}
