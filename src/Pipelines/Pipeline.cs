using Pipelines.Requests;
using Pipelines.Streams;

namespace Pipelines;

/// <summary>
/// A mediator that provides access to processing pipelines.
/// </summary>
public sealed class Pipeline
{
    private readonly IServiceProvider sp;

    /// <summary>
    /// Initializes a new instance of the <see cref="Pipeline"/> class.
    /// </summary>
    /// <param name="sp">The service provider used to resolve pipelines.</param>
    public Pipeline(IServiceProvider sp)
    {
        this.sp = sp;
    }

    /// <summary>
    /// Execute a request/response through it's corresponding pipeline.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    /// <param name="request">The request to process.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>The response.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask<TResponse> Request<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
    {
        var pipeline = sp.GetRequiredService<RequestPipeline<TRequest, TResponse>>();
        var task = pipeline.Execute(request, cancellationToken);

        return task;
    }

    /// <summary>
    /// Execute a request/response through it's corresponding pipeline.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    /// <param name="request">The request to process.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>The response.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask<TResponse> Request<TRequest, TResponse>(IRequest<TRequest, TResponse> request, CancellationToken cancellationToken = default) where TRequest : IRequest<TRequest, TResponse>
    {
        return Request<TRequest, TResponse>(Unsafe.As<IRequest<TRequest, TResponse>, TRequest>(ref request), cancellationToken);
    }

    /// <summary>
    /// Execute a request/response through it's corresponding pipeline.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    /// <param name="request">The request to process.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>An asynchronous stream of responses.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IAsyncEnumerable<TResponse> Stream<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
    {
        var pipeline = sp.GetRequiredService<StreamRequestPipeline<TRequest, TResponse>>();
        var stream = pipeline.Execute(request, cancellationToken);

        return stream;
    }

    /// <summary>
    /// Execute a request/response through it's corresponding pipeline.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    /// <param name="request">The request to process.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>An asynchronous stream of responses.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IAsyncEnumerable<TResponse> Stream<TRequest, TResponse>(IStreamRequest<TRequest, TResponse> request, CancellationToken cancellationToken = default) where TRequest : IStreamRequest<TRequest, TResponse>
    {
        return Stream<TRequest, TResponse>(Unsafe.As<IStreamRequest<TRequest, TResponse>, TRequest>(ref request), cancellationToken);
    }
}