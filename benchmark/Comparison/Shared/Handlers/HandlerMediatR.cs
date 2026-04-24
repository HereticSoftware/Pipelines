using MediatR;

namespace Comparison.Shared.Handlers;

public sealed class HandlerMediatR :
    IRequestHandler<Request, Response>,
    IStreamRequestHandler<StreamRequest, Response>
{
    public Task<Response> Handle(Request request, CancellationToken cancellationToken)
    {
        return HandlerBaseline.TaskResponse;
    }

    public IAsyncEnumerable<Response> Handle(StreamRequest request, CancellationToken cancellationToken)
    {
        return HandlerBaseline.StreamResponse(request, cancellationToken);
    }
}