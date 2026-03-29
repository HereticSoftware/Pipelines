namespace Pipelines.Streams;

/// <summary>
/// Marker interface for a request with a stream of a response type.
/// </summary>
/// <typeparam name="TSelf">The type of the request itself.</typeparam>
/// <typeparam name="TResponse">The type of the response that is to be streamed.</typeparam>
public interface IStreamRequest<TSelf, TResponse>
    where TSelf : IStreamRequest<TSelf, TResponse>
{
}
