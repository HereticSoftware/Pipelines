using Pipelines.Requests;

namespace Blazor.Client.Pages;

public sealed record IncrementCounterRequest(int Count) : IRequest<IncrementCounterRequest, IncrementCounterResponse>;

public sealed record IncrementCounterResponse(int Count, string From);

public sealed class IncrementCounterHandler : IRequestHandler<IncrementCounterRequest, IncrementCounterResponse>
{
    public ValueTask<IncrementCounterResponse> Handle(IncrementCounterRequest request, CancellationToken cancellationToken = default)
    {
        var response = new IncrementCounterResponse(request.Count + 1, "WASM");
        return new(response);
    }
}
