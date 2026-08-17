using Common;
using Microsoft.Extensions.DependencyInjection;
using Pipelines;
using Pipelines.Requests;

public static class ProgramAbstract
{
    public static async Task Run(CancellationToken ct = default)
    {
        Console.WriteLine($"""
            -----------------------------------
            Run: {nameof(ProgramAbstract)}
            -----------------------------------
            """);

        var services = new ServiceCollection();

        // These extensions methods are generated, and are put in the "Microsoft.Extensions.DependencyInjection" namespace.
        services.AddHandlers();
        services.AddPipelines();

        // Handlers are added by default, but pipeline behaviors are added manually.
        // * Registration matters as it determines the order.
        // Here are two examples.
        services.AddSingleton(typeof(IRequestBehavior<,>), typeof(GenericLoggerBehavior<,>)); // This will run 1st
        services.AddSingleton<IRequestBehavior<Ping, Result<Pong>>, PingValidator>(); // This will run 2nd

        var serviceProvider = services.BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();

        var pipeline = scope.ServiceProvider.GetRequiredService<Pipeline>();

        var id = 10; // success path
        //var id = 0; // failure path
        var request = new Ping(id);
        var response = await pipeline.Request(request, ct);

        Console.WriteLine($"""
            -----------------------------------
            ID: {id}
            Request: {request}
            Response: {(response is Result<Pong>.Ok ok ? ok.Value.Id : "failed")}
            -----------------------------------
            """);
    }

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

    public abstract class RequestGenericBehavior<TRequest, TResponse> : IRequestBehavior<TRequest, TResponse>
        where TResponse : IResult
    {
        public abstract ValueTask<TResponse> Handle(TRequest request, RequestDelegate<TRequest, TResponse> next, CancellationToken cancellationToken = default);
    }

    public abstract class RequestExplicitBehavior<TRequest, TResponse> : IRequestBehavior<TRequest, Result<TResponse>>
        where TRequest : IRequest<TRequest, Result<TResponse>>
        where TResponse : notnull
    {
        public abstract ValueTask<Result<TResponse>> Handle(TRequest request, RequestDelegate<TRequest, Result<TResponse>> next, CancellationToken cancellationToken = default);
    }

    public sealed record Ping(int Id) : Command<Ping, Pong>;

    public sealed record Pong(int Id);


    public sealed class GenericLoggerBehavior<TRequest, TResponse> : RequestGenericBehavior<TRequest, TResponse>
        where TResponse : IResult
    {
        public override async ValueTask<TResponse> Handle(TRequest request, RequestDelegate<TRequest, TResponse> next, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("1) Running logger handler");
            try
            {
                var response = await next(request, cancellationToken);
                if (response.IsEr(out var error))
                {
                    Console.WriteLine($"1) Error: {error}");
                    return response;
                }

                Console.WriteLine("1) No error!");
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"1) Error: {ex.Message}");
                return (TResponse)TResponse.CreateEr(ex.Message);
            }
        }
    }

    public sealed class PingValidator : RequestExplicitBehavior<Ping, Pong>
    {
        public override ValueTask<Result<Pong>> Handle(Ping request, RequestDelegate<Ping, Result<Pong>> next, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("2) Running ping validator");

            if (request is null || request.Id == default)
            {
                Console.WriteLine("2) Invalid input!");
                return new ValueTask<Result<Pong>>(new Result<Pong>.Er("invalid input"));
            }
            else
            {
                Console.WriteLine("2) Valid input!");
            }

            return next(request, cancellationToken);
        }
    }

    public sealed class PingHandler : CommandHandler<Ping, Pong>
    {
        public override ValueTask<Result<Pong>> Handle(Ping request, CancellationToken cancellationToken)
        {
            //throw new Exception("Failed"); // uncomment to see generic handle the error

            Console.WriteLine("3) Returning pong!");
            var result = new Result<Pong>.Ok(new(request.Id));
            return new(result);
        }
    }
}
