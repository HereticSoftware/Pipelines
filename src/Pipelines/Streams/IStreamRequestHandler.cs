namespace Pipelines.Streams;

/// <summary>
/// Defines a handler for a request.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response streamed by the handler.</typeparam>
public interface IStreamRequestHandler<TRequest, TResponse>
{
    /// <summary>
    /// Handles the specified request.
    /// </summary>
    /// <param name="request">The request to handle.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>An <see cref="IAsyncEnumerable{T}"/> of <typeparamref name="TResponse"/> representing the asynchronous stream of responses.</returns>
    IAsyncEnumerable<TResponse> Handle(TRequest request, CancellationToken cancellationToken = default);
}
