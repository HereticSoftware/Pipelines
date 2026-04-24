using Microsoft.Extensions.DependencyInjection;
using Pipelines;
using Pipelines.Requests;

public static class ProgramRequests
{
    public static async Task Run(CancellationToken ct = default)
    {
        Console.WriteLine($"""
            -----------------------------------
            Run: {nameof(ProgramRequests)}
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
        services.AddSingleton<IRequestBehavior<Ping, Pong>, PingValidator>(); // This will run 2nd

        var serviceProvider = services.BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();

        var pipeline = scope.ServiceProvider.GetRequiredService<Pipeline>();

        var id = 10;
        var request = new Ping(id);
        var response = await pipeline.Request.Execute(request, ct);

        Console.WriteLine($"""
            -----------------------------------
            ID: {id}
            Request: {request}
            Response: {response}
            -----------------------------------
            """);
    }

    public sealed record Ping(int Id) : IRequest<Ping, Pong>;

    public sealed record Pong(int Id);

    public sealed class GenericLoggerBehavior<TMessage, TResponse> : IRequestBehavior<TMessage, TResponse>
    {
        public async ValueTask<TResponse> Handle(TMessage request, RequestDelegate<TMessage, TResponse> next, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("1) Running logger handler");
            try
            {
                var response = await next(request, cancellationToken);
                Console.WriteLine("1) No error!");
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"1) Error: {ex.Message}");
                throw;
            }
        }
    }

    public sealed class PingValidator : IRequestBehavior<Ping, Pong>
    {
        public ValueTask<Pong> Handle(Ping request, RequestDelegate<Ping, Pong> next, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("2) Running ping validator");

            if (request is null || request.Id == default)
                throw new ArgumentException("Invalid input");
            else
                Console.WriteLine("2) Valid input!");

            return next(request, cancellationToken);
        }
    }

    public sealed class PingHandler : IRequestHandler<Ping, Pong>
    {
        public ValueTask<Pong> Handle(Ping request, CancellationToken cancellationToken)
        {
            Console.WriteLine("3) Returning pong!");
            return new ValueTask<Pong>(new Pong(request.Id));
        }
    }
}
