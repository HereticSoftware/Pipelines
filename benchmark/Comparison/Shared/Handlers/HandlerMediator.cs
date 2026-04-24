using Mediator;

namespace Comparison.Shared.Handlers;

public sealed class HandlerMediator :
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