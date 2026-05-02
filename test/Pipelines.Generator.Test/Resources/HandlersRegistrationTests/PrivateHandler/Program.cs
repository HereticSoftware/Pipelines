namespace Some.Nested.Types
{
    public static class Program
    {
        public static void Task Main()
        {
        }

        public sealed record Ping(Guid Id);

        public sealed record Pong(Guid Id);

        private class PingHandler : IRequestHandler<Ping, Pong>
        {
            public ValueTask<Pong> Handle(Ping request, CancellationToken cancellationToken)
            {
                return new ValueTask<Pong>(new Pong(request.Id));
            }
        }

        private class StreamPingHandler : IStreamRequestHandler<Ping, Pong>
        {
            public async IAsyncEnumerable<Pong> Handle(Ping request, CancellationToken cancellationToken)
            {
                yield break;
            }
        }
    }
}
