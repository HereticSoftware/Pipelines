using LiteBus.Queries.Abstractions;

namespace Comparison.Shared.Handlers;

public sealed class HandlerLiteBusQuery : IQueryHandler<Request, Response>
{
    public Task<Response> HandleAsync(Request message, CancellationToken cancellationToken = default)
    {
        return HandlerBaseline.TaskResponse;
    }
}
public sealed class HandlerLiteBusQueryStream : IStreamQueryHandler<StreamRequest, Response>
{
    public IAsyncEnumerable<Response> StreamAsync(StreamRequest message, CancellationToken cancellationToken = default)
    {
        return HandlerBaseline.StreamResponse(message, cancellationToken);
    }
}