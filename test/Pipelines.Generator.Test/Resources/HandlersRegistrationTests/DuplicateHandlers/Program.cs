public sealed record Ping(Guid Id) : IRequest<Ping, Pong>;

public sealed record Pong(Guid Id);

public sealed class PingHandler : IRequestHandler<Ping, Pong>
{
    public ValueTask<Pong> Handle(Ping request, CancellationToken cancellationToken)
    {
        return new ValueTask<Pong>(new Pong(request.Id));
    }
}

public sealed class DuplicatePingHandler : IRequestHandler<Ping, Pong>
{
    public ValueTask<Pong> Handle(Ping request, CancellationToken cancellationToken)
    {
        return new ValueTask<Pong>(new Pong(request.Id));
    }
}

public sealed class PingStreamHandler : IStreamRequestHandler<Ping, Pong>
{
    public async IAsyncEnumerable<Pong> Handle(Ping request, CancellationToken cancellationToken)
    {
        yield break;
    }
}

public sealed class DuplicateStreamPingHandler : IStreamRequestHandler<Ping, Pong>
{
    public async IAsyncEnumerable<Pong> Handle(Ping request, CancellationToken cancellationToken)
    {
        yield break;
    }
}
