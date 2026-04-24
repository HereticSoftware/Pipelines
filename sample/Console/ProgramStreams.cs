using Microsoft.Extensions.DependencyInjection;
using Pipelines;
using Pipelines.Streams;
using System.Runtime.CompilerServices;

public static class ProgramStreams
{
    public static async Task Run(CancellationToken ct = default)
    {
        Console.WriteLine($"""
            -----------------------------------
            Run: {nameof(ProgramStreams)}
            -----------------------------------
            """);

        var services = new ServiceCollection();

        // These extensions methods are generated, and are put in the "Microsoft.Extensions.DependencyInjection" namespace.
        services.AddHandlers();
        services.AddPipelines();

        // Handlers are added by default, but pipeline behaviors are added manually.
        // * Registration matters as it determines the order.
        // Here are two examples.
        services.AddSingleton(typeof(IStreamRequestBehavior<,>), typeof(GenericLoggerBehavior<,>)); // This will run 1st
        services.AddSingleton<IStreamRequestBehavior<Ping, Pong>, PingValidator>(); // This will run 2nd
        services.AddSingleton<IStreamRequestBehavior<Ping, Pong>, PingTagger>(); // This will run 3rd

        var serviceProvider = services.BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();

        var pipeline = scope.ServiceProvider.GetRequiredService<Pipeline>();

        var id = 20;
        var request = new Ping(id);
        await foreach (var response in pipeline.Stream.Execute(request, ct))
        {
            Console.WriteLine($"""
                -----------------------------------
                ID: {id}
                Request: {request}
                Response: {response}
                -----------------------------------
                """);
        }
    }

    public sealed record Ping(int Id) : IStreamRequest<Ping, Pong>;

    public sealed record Pong(int Id, string? Tag = null);

    public sealed class GenericLoggerBehavior<TMessage, TResponse> : IStreamRequestBehavior<TMessage, TResponse>
    {
        public IAsyncEnumerable<TResponse> Handle(TMessage request, StreamRequestDelegate<TMessage, TResponse> next, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("1) Logger handler");
            return next(request, cancellationToken);
        }
    }

    public sealed class PingValidator : IStreamRequestBehavior<Ping, Pong>
    {
        public IAsyncEnumerable<Pong> Handle(Ping request, StreamRequestDelegate<Ping, Pong> next, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("2) Running ping validator");

            if (request is null || request.Id == default)
                throw new ArgumentException("Invalid input");
            else
                Console.WriteLine("2) Valid input!");

            return next(request, cancellationToken);
        }
    }

    public sealed class PingTagger : IStreamRequestBehavior<Ping, Pong>
    {
        public async IAsyncEnumerable<Pong> Handle(Ping request, StreamRequestDelegate<Ping, Pong> next, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await foreach (var response in next(request, cancellationToken))
            {
                Console.WriteLine("3) Tagging pong");
                yield return response with { Tag = "Tagger" };
            }
        }
    }

    public sealed class PingHandler : IStreamRequestHandler<Ping, Pong>
    {
        public async IAsyncEnumerable<Pong> Handle(Ping request, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            Console.WriteLine("4) Returning 3 pongs!");

            yield return new Pong(request.Id);
            yield return new Pong(request.Id);
            yield return new Pong(request.Id);
        }
    }
}
