namespace Pipelines.Test.Integration;

public class InsideClass
{
    public sealed class Ping;

    public sealed class Pong;

    public sealed class PingHandler : IRequestHandler<Ping, Pong>
    {
        public ValueTask<Pong> Handle(Ping request, CancellationToken cancellationToken)
        {
            var Pong = new Pong();
            return new(Pong);
        }
    }
}
