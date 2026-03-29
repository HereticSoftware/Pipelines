namespace Pipelines.Requests;

/// <summary>
/// Defines a pipeline behavior for processing a request.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public interface IRequestBehavior<TRequest, TResponse>
{
    /// <summary>
    /// Handles the request and invokes the next delegate in the pipeline.
    /// </summary>
    /// <param name="request">The request instance.</param>
    /// <param name="next">The next delegate in the pipeline.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A <see cref="ValueTask{TResponse}"/> representing the asynchronous operation.</returns>
    ValueTask<TResponse> Handle(TRequest request, RequestDelegate<TRequest, TResponse> next, CancellationToken cancellationToken = default);
}
