namespace Some.Very.Very.Very.Very.Deep.Namespace.ThatIUseToTestTheSourceGenSoThatItCanHandleLotsOfDifferentInput
{
    public sealed record Ping(Guid Id) : IRequest<Ping, Pong>;

    public sealed record Pong(Guid Id);

    public sealed class PingHandler : IRequestHandler<Ping, Pong>
    {
        public ValueTask<Pong> Handle(Ping request, CancellationToken cancellationToken)
        {
            return new ValueTask<Pong>(new Pong(request.Id));
        }
    }
}
