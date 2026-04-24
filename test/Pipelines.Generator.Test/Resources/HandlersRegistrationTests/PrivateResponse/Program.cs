namespace Some.Nested.Types
{
    public static class Program
    {
        public static void Task Main()
        {
        }

        public sealed record Ping(Guid Id);

        private sealed record Pong(Guid Id);

        public class PingHandler : IRequestHandler<Ping, Pong>
        {
            public ValueTask<Pong> Handle(Ping request, CancellationToken cancellationToken)
            {
                return new ValueTask<Pong>(new Pong(request.Id));
            }
        }
    }
}
