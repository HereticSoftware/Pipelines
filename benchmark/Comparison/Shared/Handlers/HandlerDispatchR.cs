using DispatchR.Abstractions.Send;
using DispatchR.Abstractions.Stream;

namespace Comparison.Shared.Handlers;

public sealed class HandlerDispatchR :
    IRequestHandler<Request, ValueTask<Response>>,
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