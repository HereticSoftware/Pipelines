namespace Comparison.Benchmarks;

[BenchmarkCategory("request")]
public class RequestBenchmarks : BenchmarksBase
{
    private readonly Request Request = new(Guid.NewGuid());

    [Benchmark(Baseline = true)]
    public ValueTask<Response> Baseline()
    {
        return Mediators.Baseline.Baseline(Request, CancellationToken.None);
    }

    [Benchmark]
    public ValueTask<Response> Pipeline()
    {
        return Mediators.Pipeline.Request(Request, CancellationToken.None);
    }

    [Benchmark]
    public ValueTask<Response> Pipeline_Raw()
    {
        return Mediators.PipelineRequestRaw.Execute(Request, CancellationToken.None);
    }

    [Benchmark]
    public Task<Response> MediatR()
    {
        return Mediators.MediatrInterface.Send(Request, CancellationToken.None);
    }

    [Benchmark]
    public ValueTask<Response> Mediator_Interface()
    {
        return Mediators.MediatorInterface.Send(Request, CancellationToken.None);
    }

    [Benchmark]
    public ValueTask<Response> Mediator_Concrete()
    {
        return Mediators.MediatorConcrete.Send(Request, CancellationToken.None);
    }
}
