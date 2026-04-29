using Pipelines;
using Pipelines.Streams;
using System.Runtime.CompilerServices;

namespace Pipelines.Generator.Test.Integration;

public sealed class StreamRequestHandlerTests : TestBase
{
    [Test]
    public async Task Should_Register_Handler()
    {
        var services = SetupServices();

        var pingHandler = services.GetService<IStreamRequestHandler<Ping, Pong>>();
        await That(pingHandler).IsNotNull();

        var pongHandler = services.GetService<IStreamRequestHandler<Pong, Ping>>();
        await That(pongHandler).IsNotNull();
    }

    [Test]
    public async Task Should_Register_Pipeline()
    {
        var services = SetupServices();

        var pingPipline = services.GetService<StreamRequestPipeline<Ping, Pong>>();
        await That(pingPipline).IsNotNull();

        var pongPipline = services.GetService<StreamRequestPipeline<Pong, Ping>>();
        await That(pongPipline).IsNotNull();
    }

    [Test]
    public async Task Should_Execute_Pipeline()
    {
        var services = SetupServices();

        var pipline = services.GetService<Pipeline>();
        await That(pipline).IsNotNull();

        var pongs = await pipline!.Stream(new Ping()).ToArrayAsync();
        await That(pongs).Count().IsEqualTo(2);

        var pings = await pipline!.Stream(new Pong(10)).ToArrayAsync();
        await That(pings).Count().IsEqualTo(2);
    }

    [Test]
    public async Task Should_Execute_Behaviors_In_Order()
    {
        var services = SetupServices(services =>
        {
            services.AddScoped(typeof(IStreamRequestBehavior<,>), typeof(Behavior1<,>));
            services.AddScoped(typeof(IStreamRequestBehavior<,>), typeof(Behavior2<,>));
            services.AddScoped(typeof(IStreamRequestBehavior<,>), typeof(Behavior3<,>));
        });

        var context = services.GetRequiredService<RequestContext>();
        var pipline = services.GetService<Pipeline>();
        await That(pipline).IsNotNull();

        var pongs = await pipline!.Stream(new Ping()).ToArrayAsync();
        await That(pongs).Count().IsEqualTo(2);

        var stack = context.CallStack.ToArray();

        await That(stack).Count().IsEqualTo(4);
        await That(stack[0]).IsEqualTo(nameof(PingHandler));
        await That(stack[1]).IsEqualTo("Behavior3");
        await That(stack[2]).IsEqualTo("Behavior2");
        await That(stack[3]).IsEqualTo("Behavior1");
    }

    public sealed class Ping : IStreamRequest<Ping, Pong>;

    public sealed record Pong(int Num) : IStreamRequest<Pong, Ping>;

    public sealed class PingHandler(RequestContext context) : IStreamRequestHandler<Ping, Pong>, IStreamRequestHandler<Pong, Ping>
    {
        public async IAsyncEnumerable<Pong> Handle(Ping request, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            context.CallStack.Push(GetType().Name);

            await Task.CompletedTask;

            yield return new Pong(0);
            yield return new Pong(0);
        }

        public async IAsyncEnumerable<Ping> Handle(Pong request, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            context.CallStack.Push(GetType().Name);

            await Task.CompletedTask;

            yield return new Ping();
            yield return new Ping();
        }
    }

    public sealed class Behavior1<TRequest, TResponse>(RequestContext context) : IStreamRequestBehavior<TRequest, TResponse>
    {
        public IAsyncEnumerable<TResponse> Handle(TRequest request, StreamRequestDelegate<TRequest, TResponse> next, CancellationToken cancellationToken = default)
        {
            context.CallStack.Push("Behavior1");
            return next(request, cancellationToken);
        }
    }

    public sealed class Behavior2<TRequest, TResponse>(RequestContext context) : IStreamRequestBehavior<TRequest, TResponse>
    {
        public IAsyncEnumerable<TResponse> Handle(TRequest request, StreamRequestDelegate<TRequest, TResponse> next, CancellationToken cancellationToken = default)
        {
            context.CallStack.Push("Behavior2");
            return next(request, cancellationToken);
        }
    }

    public sealed class Behavior3<TRequest, TResponse>(RequestContext context) : IStreamRequestBehavior<TRequest, TResponse>
    {
        public IAsyncEnumerable<TResponse> Handle(TRequest request, StreamRequestDelegate<TRequest, TResponse> next, CancellationToken cancellationToken = default)
        {
            context.CallStack.Push("Behavior3");
            return next(request, cancellationToken);
        }
    }
}
