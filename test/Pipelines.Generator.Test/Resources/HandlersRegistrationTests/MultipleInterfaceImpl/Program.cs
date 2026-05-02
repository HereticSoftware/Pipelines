namespace SomeProgram;

public sealed record Ping1();

public sealed record Ping2();

public sealed record Pong();

public class Ping1Handler :
    IRequestHandler<Ping1, Pong>,
    IRequestHandler<Ping2, Pong>
{
    public ValueTask<Pong> Handle(Ping1 request, CancellationToken cancellationToken)
    {
        return new ValueTask<Pong>(new Pong());
    }

    public ValueTask<Pong> Handle(Ping2 request, CancellationToken cancellationToken)
    {
        return new ValueTask<Pong>(new Pong());
    }
}

public class StreamPing1Handler :
    IStreamRequestHandler<Ping1, Pong>,
    IStreamRequestHandler<Ping2, Pong>
{
    public async IAsyncEnumerable<Pong> Handle(Ping1 request, CancellationToken cancellationToken)
    {
        yield break;
    }

    public async IAsyncEnumerable<Pong> Handle(Ping2 request, CancellationToken cancellationToken)
    {
        yield break;
    }
}
