namespace Pipelines.Requests;

/// <summary>
/// Defines a handler for a request.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response returned by the handler.</typeparam>
public interface IRequestHandler<TRequest, TResponse>
{
    /// <summary>
    /// Handles the specified request.
    /// </summary>
    /// <param name="request">The request to handle.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A <see cref="ValueTask{TResponse}"/> representing the asynchronous operation.</returns>
    ValueTask<TResponse> Handle(TRequest request, CancellationToken cancellationToken = default);
}
