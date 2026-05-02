namespace Pipelines.Test.Integration;

public static class Program
{
    public static async Task Main()
    {
    }
}

// Define base convention of Command and command handler
public sealed class Result<TResult>;

public abstract record Command<TSelf, TResult> : IRequest<TSelf, Result<TResult>>
    where TSelf : Command<TSelf, TResult>
    where TResult : notnull;


public abstract class CommandHandler<TCommand, TResult> :
    IRequestHandler<TCommand, Result<TResult>>
    where TCommand : Command<TCommand, TResult>
    where TResult : notnull
{
    public abstract ValueTask<Result<TResult>> Handle(TCommand command, CancellationToken cancellationToken);
}

public abstract class StreamCommandHandler<TCommand, TResult> :
    IStreamRequestHandler<TCommand, Result<TResult>>
    where TCommand : Command<TCommand, TResult>
    where TResult : notnull
{
    public abstract IAsyncEnumerable<Result<TResult>> Handle(TCommand command, CancellationToken cancellationToken);
}

// Define class from convention
public sealed record Pong;

public sealed record Ping : Command<Ping, Pong>;

public sealed class PingCommandHandler : CommandHandler<Ping, Pong>
{
    public override ValueTask<Result<Pong>> Handle(Ping Ping, CancellationToken cancellationToken)
    {
        var r = new Result<Pong>();
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
