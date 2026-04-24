namespace Comparison.Benchmarks;

[BenchmarkCategory("cold")]
public class ColdBenchmarks : BenchmarksBase
{
    private readonly Request Request = new(Guid.NewGuid());

    [Benchmark(Baseline = true)]
    public ValueTask<Response> Baseline()
    {
        using var scope = Host.Services.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<HandlerBaseline>();

        return handler.Baseline(Request, CancellationToken.None);
    }

    [Benchmark]
    public ValueTask<Response> Pipeline()
    {
        using var scope = Host.Services.CreateScope();
        var pipeline = scope.ServiceProvider.GetRequiredService<Pipelines.Pipeline>();

        return pipeline.Request(Request, CancellationToken.None);
    }

    [Benchmark]
    public ValueTask<Response> Pipeline_Raw()
    {
        using var scope = Host.Services.CreateScope();
        var pipeline = scope.ServiceProvider.GetRequiredService<Pipelines.Requests.RequestPipeline<Request, Response>>();

        return pipeline.Execute(Request, CancellationToken.None);
    }

    [Benchmark]
    public Task<Response> MediatR()
    {
        using var scope = Host.Services.CreateScope();
        var mediatr = scope.ServiceProvider.GetRequiredService<MediatR.IMediator>();

        return mediatr.Send(Request, CancellationToken.None);
    }

    [Benchmark]
    public ValueTask<Response> Mediator_Interface()
    {
        using var scope = Host.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<Mediator.IMediator>();

        return mediator.Send(Request, CancellationToken.None);
    }

    [Benchmark]
    public ValueTask<Response> Mediator_Concrete()
    {
        using var scope = Host.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<Mediator.Mediator>();

        return mediator.Send(Request, CancellationToken.None);
    }

    [Benchmark]
    public ValueTask<Response> DispatchR()
    {
        using var scope = Host.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<DispatchR.IMediator>();

        return mediator.Send(Request, CancellationToken.None);
    }

    [Benchmark]
    public Task<Response> LiteBus()
    {
        using var scope = Host.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<LiteBus.Queries.Abstractions.IQueryMediator>();

        return mediator.QueryAsync(Request, cancellationToken: CancellationToken.None);
    }
}
