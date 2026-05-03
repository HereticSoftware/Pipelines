public sealed record Ping(Guid Id) : IRequest<Ping, Pong>, IStreamRequest<Ping, Pong>;

public sealed record Pong(Guid Id);

public readonly struct PingHandler : IRequestHandler<Ping, Pong>
{
    public ValueTask<Pong> Handle(Ping request, CancellationToken cancellationToken)
    {
        return new ValueTask<Pong>(new Pong(request.Id));
    }
}

public readonly struct PingStreamHandler : IStreamRequestHandler<Ping, Pong>
{
    public async IAsyncEnumerable<Pong> Handle(Ping request, CancellationToken cancellationToken)
    {
        yield break;
    }
}
