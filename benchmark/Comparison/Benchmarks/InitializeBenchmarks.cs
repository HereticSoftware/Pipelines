namespace Comparison.Benchmarks;

[BenchmarkCategory("init")]
public class InitializeBenchmarks : BenchmarksBase
{
    [Benchmark(Baseline = true)]
    public HandlerBaseline Baseline()
    {
        using var scope = Host.Services.CreateScope();

        return scope.ServiceProvider.GetRequiredService<HandlerBaseline>();
    }

    [Benchmark]
    public Pipelines.Pipeline Pipeline()
    {
        using var scope = Host.Services.CreateScope();
        var pipeline = scope.ServiceProvider.GetRequiredService<Pipelines.Pipeline>();

        return pipeline;
    }

    [Benchmark]
    public Pipelines.Requests.RequestPipeline<Request, Response> Pipeline_Raw()
    {
        using var scope = Host.Services.CreateScope();
        var pipeline = scope.ServiceProvider.GetRequiredService<Pipelines.Requests.RequestPipeline<Request, Response>>();

        return pipeline;
    }

    [Benchmark]
    public MediatR.IMediator MediatR()
    {
        using var scope = Host.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<MediatR.IMediator>();

        return mediator;
    }

    [Benchmark]
    public Mediator.IMediator Mediator_Interface()
    {
        using var scope = Host.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<Mediator.IMediator>();

        return mediator;
    }

    [Benchmark]
    public Mediator.IMediator Mediator_Concrete()
    {
        using var scope = Host.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<Mediator.Mediator>();

        return mediator;
    }

    [Benchmark]
    public DispatchR.IMediator DispatchR()
    {
        using var scope = Host.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<DispatchR.IMediator>();

        return mediator;
    }

    [Benchmark]
    public LiteBus.Queries.Abstractions.IQueryMediator LiteBus()
    {
        using var scope = Host.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<LiteBus.Queries.Abstractions.IQueryMediator>();

        return mediator;
    }
}
