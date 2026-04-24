using Pipelines.Requests;
using Pipelines.Streams;

namespace Comparison.Shared.Handlers;

public sealed class HandlerPipeline :
    IRequestHandler<Request, Response>,
    IStreamRequestHandler<StreamRequest, Response>
{
    public ValueTask<Response> Handle(Request request, CancellationToken cancellationToken)
    {
        return new(HandlerBaseline.Response);
    }

    public IAsyncEnumerable<Response> Handle(StreamRequest request, CancellationToken cancellationToken)
    {
        return HandlerBaseline.StreamResponse(request, cancellationToken);
    }
}