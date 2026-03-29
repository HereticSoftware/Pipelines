namespace Pipelines.Streams;

/// <summary>
/// Represents a delegate in the request pipeline.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
/// <param name="request">The request instance.</param>
/// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
/// <returns>An <see cref="IAsyncEnumerable{T}"/> of <typeparamref name="TResponse"/> representing the asynchronous stream of responses.</returns>
public delegate IAsyncEnumerable<TResponse> StreamRequestDelegate<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default);
