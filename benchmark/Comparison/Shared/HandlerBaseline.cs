namespace Comparison.Shared;

public sealed class HandlerBaseline
{
    public static Response Response { get; } = new(Guid.NewGuid());

    public static Task<Response> TaskResponse { get; } = Task.FromResult(Response);

    public static async IAsyncEnumerable<Response> StreamResponse(StreamRequest request, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        for (int i = 0; i < 3; i++)
        {
            await Task.CompletedTask;
            yield return Response;
        }
    }

    public ValueTask<Response> Baseline(Request request, CancellationToken cancellationToken)
    {
        return new(Response);
    }

    public IAsyncEnumerable<Response> Baseline(StreamRequest request, CancellationToken cancellationToken)
    {
        return StreamResponse(request, cancellationToken);
    }
}