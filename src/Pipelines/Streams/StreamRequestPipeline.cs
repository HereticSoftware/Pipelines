namespace Pipelines.Streams;

/// <summary>
/// Represents a composable request pipeline that processes requests through a series of behaviors before reaching the core handler.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public sealed class StreamRequestPipeline<TRequest, TResponse>
{
    private readonly StreamRequestDelegate<TRequest, TResponse> handle;

    /// <summary>
    /// Initializes a new instance of the <see cref="StreamRequestPipeline{TRequest, TResponse}"/> class.
    /// </summary>
    /// <param name="handler">The core request handler that will be invoked after all behaviors in the pipeline.</param>
    /// <param name="behaviors">An enumerable collection of request behaviors to execute in the pipeline.</param>
    public StreamRequestPipeline(IStreamRequestHandler<TRequest, TResponse> handler, IEnumerable<IStreamRequestBehavior<TRequest, TResponse>> behaviors)
    {
        var handle = (StreamRequestDelegate<TRequest, TResponse>)handler.Handle;
        if (behaviors is IStreamRequestBehavior<TRequest, TResponse>[])
        {
            var behaviorsArray = Unsafe.As<IEnumerable<IStreamRequestBehavior<TRequest, TResponse>>, IStreamRequestBehavior<TRequest, TResponse>[]>(ref behaviors);
            var length = behaviorsArray.Length;
            for (int i = length - 1; i >= 0; i--)
            {
                var behavior = behaviorsArray[i];
                var handleCopy = handle;
                handle = (request, cancellationToken) => behavior.Handle(request, handleCopy, cancellationToken);
            }
        }
        else
        {
            foreach (var behavior in behaviors.Reverse())
            {
                var handleCopy = handle;
                var behaviorCopy = behavior;
                handle = (request, cancellationToken) => behaviorCopy.Handle(request, handleCopy, cancellationToken);
            }
        }
        this.handle = handle;
    }

    /// <summary>
    /// Executes the request through the pipeline.
    /// </summary>
    /// <param name="request">The request to handle.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>An <see cref="IAsyncEnumerable{T}"/> of <typeparamref name="TResponse"/> representing the asynchronous stream of responses.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IAsyncEnumerable<TResponse> Execute(TRequest request, CancellationToken cancellationToken = default)
    {
        return handle(request, cancellationToken);
    }
}
