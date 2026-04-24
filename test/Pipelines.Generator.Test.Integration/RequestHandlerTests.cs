using Pipelines;
using Pipelines.Requests;

namespace Pipelines.Generator.Test.Integration;

public sealed class RequestHandlerTests : TestBase
{
    [Test]
    public async Task Should_Register_Handler()
    {
        var services = SetupServices();

        var pingHandler = services.GetService<IRequestHandler<Ping, Pong>>();
        await That(pingHandler).IsNotNull();

        var pongHandler = services.GetService<IRequestHandler<Pong, Ping>>();
        await That(pongHandler).IsNotNull();
    }

    [Test]
    public async Task Should_Register_Pipeline()
    {
        var services = SetupServices();

        var pingPipline = services.GetService<RequestPipeline<Ping, Pong>>();
        await That(pingPipline).IsNotNull();

        var pongPipline = services.GetService<RequestPipeline<Pong, Ping>>();
        await That(pongPipline).IsNotNull();
    }

    [Test]
    public async Task Should_Execute_Pipeline()
    {
        var services = SetupServices();

        var pipline = services.GetService<Pipeline>();
        await That(pipline).IsNotNull();

        var pong = await pipline!.Request.Execute(new Ping());
        await That(pong).IsNotNull();

        var ping = await pipline!.Request.Execute(new Pong(10));
        await That(ping).IsNotNull();
    }

    [Test]
    public async Task Should_Execute_Behaviors_In_Order()
    {
        var services = SetupServices(services =>
        {
            services.AddScoped(typeof(IRequestBehavior<,>), typeof(Behavior1<,>));
            services.AddScoped(typeof(IRequestBehavior<,>), typeof(Behavior2<,>));
            services.AddScoped(typeof(IRequestBehavior<,>), typeof(Behavior3<,>));
        });

        var context = services.GetRequiredService<RequestContext>();
        var pipline = services.GetService<Pipeline>();
        await That(pipline).IsNotNull();

        var pong = await pipline!.Request.Execute(new Ping());
        await That(pong).IsNotNull();

        var stack = context.CallStack.ToArray();

        await That(stack).Count().IsEqualTo(4);
        await That(stack[0]).IsEqualTo(nameof(PingHandler));
        await That(stack[1]).IsEqualTo("Behavior3");
        await That(stack[2]).IsEqualTo("Behavior2");
        await That(stack[3]).IsEqualTo("Behavior1");
    }

    public sealed class Ping : IRequest<Ping, Pong>;

    public sealed record Pong(int Num) : IRequest<Pong, Ping>;

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

    public sealed class Behavior1<TRequest, TResponse>(RequestContext context) : IRequestBehavior<TRequest, TResponse>
    {
        public ValueTask<TResponse> Handle(TRequest request, RequestDelegate<TRequest, TResponse> next, CancellationToken cancellationToken = default)
        {
            context.CallStack.Push("Behavior1");
            return next(request, cancellationToken);
        }
    }

    public sealed class Behavior2<TRequest, TResponse>(RequestContext context) : IRequestBehavior<TRequest, TResponse>
    {
        public ValueTask<TResponse> Handle(TRequest request, RequestDelegate<TRequest, TResponse> next, CancellationToken cancellationToken = default)
        {
            context.CallStack.Push("Behavior2");
            return next(request, cancellationToken);
        }
    }

    public sealed class Behavior3<TRequest, TResponse>(RequestContext context) : IRequestBehavior<TRequest, TResponse>
    {
        public ValueTask<TResponse> Handle(TRequest request, RequestDelegate<TRequest, TResponse> next, CancellationToken cancellationToken = default)
        {
            context.CallStack.Push("Behavior3");
            return next(request, cancellationToken);
        }
    }
}
