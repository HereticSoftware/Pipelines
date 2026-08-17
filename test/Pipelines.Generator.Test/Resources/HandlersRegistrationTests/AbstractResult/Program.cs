namespace Pipelines.Test.Integration;

public static class Program
{
    public static async Task Main()
    {
    }
}

// Define base convention of Command and command handler
public abstract record Result<TResult> where TResult : notnull;

public sealed record Ok<TResult>(TResult Result) : Result<TResult> where TResult : notnull;

public abstract record Command<TSelf, TResponse> : IRequest<TSelf, Result<TResponse>>
    where TSelf : Command<TSelf, TResponse>
    where TResponse : notnull
{
}

public abstract class CommandHandler<TRequest, TResponse> : IRequestHandler<TRequest, Result<TResponse>>
    where TRequest : Command<TRequest, TResponse>
    where TResponse : notnull
{
    public abstract ValueTask<Result<TResponse>> Handle(TRequest request, CancellationToken cancellationToken = default);
}

public abstract class StreamCommandHandler<TRequest, TResponse> : IStreamRequestHandler<TRequest, Result<TResponse>>
    where TRequest : Command<TRequest, TResponse>
    where TResponse : notnull
{
    public abstract IAsyncEnumerable<Result<TResponse>> Handle(TRequest command, CancellationToken cancellationToken);
}

// Define class from convention
public sealed record Pong;

public sealed record Ping : Command<Ping, Pong>;

public sealed class PingCommandHandler : CommandHandler<Ping, Pong>
{
    public override ValueTask<Result<Pong>> Handle(Ping Ping, CancellationToken cancellationToken)
    {
        var r = new Ok<Pong>(new Pong());
        return new(r);
    }
}

public sealed class PingStreamCommandHandler : StreamCommandHandler<Ping, Pong>
{
    public override async IAsyncEnumerable<Result<Pong>> Handle(Ping Ping, CancellationToken cancellationToken)
    {
        yield break;
    }
}
