namespace Pipelines.Streams;

/// <summary>
/// Defines a pipeline behavior for processing a request.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public interface IStreamRequestBehavior<TRequest, TResponse>
{
    /// <summary>
    /// Handles the request and invokes the next delegate in the pipeline.
    /// </summary>
    /// <param name="request">The request instance.</param>
    /// <param name="next">The next delegate in the pipeline.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>An <see cref="IAsyncEnumerable{T}"/> of <typeparamref name="TResponse"/> representing the asynchronous stream of responses.</returns>
    IAsyncEnumerable<TResponse> Handle(TRequest request, StreamRequestDelegate<TRequest, TResponse> next, [EnumeratorCancellation] CancellationToken cancellationToken = default);
}
