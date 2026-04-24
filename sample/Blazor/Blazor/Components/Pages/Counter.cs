using Blazor.Client.Pages;
using Pipelines.Requests;

namespace Blazor.Components.Pages;

// The AddHandlers method is different for each assembly. Meaning it's project has its own AddHandlers internal method.
// For auto rendering one would need to implement a handler for client and server as you might be in the server rendering at the start
// which would result in a service not found exception as no handler exists in the client project.
// this handler fills this gap and performs the correct behavior when we use server rendering instead of wasm.
public sealed class IncrementCounterHandler : IRequestHandler<IncrementCounterRequest, IncrementCounterResponse>
{
    public ValueTask<IncrementCounterResponse> Handle(IncrementCounterRequest request, CancellationToken cancellationToken = default)
    {
        var response = new IncrementCounterResponse(request.Count + 1, "SERVER");
        return new(response);
    }
}
