namespace Some.Nested.Types.One
{
    public sealed record Ping(Guid Id) : IRequest<Ping, byte[]>, IStreamRequest<Ping, byte[]>;

    public sealed class PingHandler : IRequestHandler<Ping, byte[]>
    {
        public ValueTask<byte[]> Handle(Ping request, CancellationToken cancellationToken)
        {
            var bytes = request.Id.ToByteArray();
            return new ValueTask<byte[]>(bytes);
        }
    }

    public class PingStreamHandler : IStreamRequestHandler<Ping, byte[]>
    {
        public async IAsyncEnumerable<byte[]> Handle(Ping request, CancellationToken cancellationToken)
        {
            yield break;
        }
    }
}

namespace Some.Nested.Types.Two
{
    public sealed record Ping(Guid Id) : IRequest<Ping, byte[]>, IStreamRequest<Ping, byte[]>;

    public sealed class PingHandler : IRequestHandler<Ping, byte[]>
    {
        public ValueTask<byte[]> Handle(Ping request, CancellationToken cancellationToken)
        {
            var bytes = request.Id.ToByteArray();
            return new ValueTask<byte[]>(bytes);
        }
    }

    public class PingStreamHandler : IStreamRequestHandler<Ping, byte[]>
    {
        public async IAsyncEnumerable<byte[]> Handle(Ping request, CancellationToken cancellationToken)
        {
            yield break;
        }
    }
}
