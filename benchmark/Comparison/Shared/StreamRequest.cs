namespace Comparison.Shared;

public sealed record StreamRequest(Guid Id) :
    Pipelines.Streams.IStreamRequest<StreamRequest, Response>,
    MediatR.IStreamRequest<Response>,
    Mediator.IStreamRequest<Response>,
    DispatchR.Abstractions.Stream.IStreamRequest<Request, Response>,
    LiteBus.Queries.Abstractions.IStreamQuery<Response>
    ;
