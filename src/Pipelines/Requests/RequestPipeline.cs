namespace Pipelines.Requests;

/// <summary>
/// Represents a composable request pipeline that processes requests through a series of behaviors before reaching the core handler.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public sealed class RequestPipeline<TRequest, TResponse>
{
    private readonly RequestDelegate<TRequest, TResponse> handle;

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestPipeline{TRequest, TResponse}"/> class.
    /// </summary>
    /// <param name="handler">The core request handler that will be invoked after all behaviors in the pipeline.</param>
    /// <param name="behaviors">An enumerable collection of request behaviors to execute in the pipeline.</param>
    public RequestPipeline(IRequestHandler<TRequest, TResponse> handler, IEnumerable<IRequestBehavior<TRequest, TResponse>> behaviors)
    {
        var handle = (RequestDelegate<TRequest, TResponse>)handler.Handle;
        if (behaviors is IRequestBehavior<TRequest, TResponse>[])
        {
            var behaviorsArray = Unsafe.As<IEnumerable<IRequestBehavior<TRequest, TResponse>>, IRequestBehavior<TRequest, TResponse>[]>(ref behaviors);
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
    /// <returns>A <see cref="ValueTask{TResponse}"/> representing the asynchronous operation, containing the response.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask<TResponse> Execute(TRequest request, CancellationToken cancellationToken = default)
    {
        return handle(request, cancellationToken);
    }
}
