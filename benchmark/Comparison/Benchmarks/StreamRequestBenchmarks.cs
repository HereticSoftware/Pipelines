namespace Comparison.Benchmarks;

[BenchmarkCategory("stream")]
public class StreamRequestBenchmarks : BenchmarksBase
{
    private readonly StreamRequest StreamRequest = new(Guid.NewGuid());

    [Benchmark(Baseline = true)]
    public async ValueTask Baseline()
    {
        await foreach (var response in Mediators.Baseline.Baseline(StreamRequest, CancellationToken.None))
        {
            _ = response;
        }
    }

    [Benchmark]
    public async ValueTask Pipeline()
    {
        await foreach (var response in Mediators.Pipeline.Stream(StreamRequest, CancellationToken.None))
        {
            _ = response;
        }
    }

    [Benchmark]
    public async ValueTask Pipeline_Raw()
    {
        await foreach (var response in Mediators.PipelineStreamRaw.Execute(StreamRequest, CancellationToken.None))
        {
            _ = response;
        }
    }

    [Benchmark]
    public async ValueTask MediatR()
    {
        await foreach (var response in Mediators.MediatrInterface.CreateStream(StreamRequest, CancellationToken.None))
        {
            _ = response;
        }
    }

    [Benchmark]
    public async ValueTask Mediator_Interface()
    {
        await foreach (var response in Mediators.MediatorInterface.CreateStream(StreamRequest, CancellationToken.None))
        {
            _ = response;
        }
    }

    [Benchmark]
    public async ValueTask Mediator_Concrete()
    {
        await foreach (var response in Mediators.MediatorConcrete.CreateStream(StreamRequest, CancellationToken.None))
        {
            _ = response;
        }
    }
}
