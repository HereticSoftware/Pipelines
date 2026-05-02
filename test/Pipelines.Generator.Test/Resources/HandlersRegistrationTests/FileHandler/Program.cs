namespace Some.Nested.Types
{
    public sealed record Ping(Guid Id);

    public sealed record Pong(Guid Id);

    file class PingHandler : IRequestHandler<Ping, Pong>
    {
        public ValueTask<Pong> Handle(Ping request, CancellationToken cancellationToken)
        {
            return new ValueTask<Pong>(new Pong(request.Id));
        }
    }

    file class PingStreamHandler : IStreamRequestHandler<Ping, Pong>
    {
        public async IAsyncEnumerable<Pong> Handle(Ping request, CancellationToken cancellationToken)
        {
            yield break;
        }
    }
}
