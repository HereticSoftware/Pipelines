namespace Comparison.Shared;

public sealed record Request(Guid Id) :
    Pipelines.Requests.IRequest<Request, Response>,
    MediatR.IRequest<Response>,
    Mediator.IRequest<Response>,
    DispatchR.Abstractions.Send.IRequest<Request, ValueTask<Response>>,
    LiteBus.Queries.Abstractions.IQuery<Response>
    ;
