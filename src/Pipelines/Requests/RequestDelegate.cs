namespace Pipelines.Requests;

/// <summary>
/// Represents a delegate in the request pipeline.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
/// <param name="request">The request instance.</param>
/// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
/// <returns>A <see cref="ValueTask{TResponse}"/> representing the asynchronous operation.</returns>
public delegate ValueTask<TResponse> RequestDelegate<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default);
