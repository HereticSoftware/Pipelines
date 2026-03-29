namespace Pipelines.Requests;

/// <summary>
/// Marker interface for a request with a response type.
/// </summary>
/// <typeparam name="TSelf">The type of the request itself.</typeparam>
/// <typeparam name="TResponse">The type of the response returned by the request.</typeparam>
public interface IRequest<TSelf, TResponse>
    where TSelf : IRequest<TSelf, TResponse>
{
}
