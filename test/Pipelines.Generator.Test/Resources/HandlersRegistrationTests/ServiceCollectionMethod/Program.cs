using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Some;

public static class Program
{
    public static async Task Main()
    {
        var services = new ServiceCollection();

        services.AddHandlers();

        var serviceProvider = services.BuildServiceProvider();

        var pipeline = serviceProvider.GetRequiredService<Pipeline>();

        _ = await pipeline.Send(new Ping(Guid.NewGuid()));
    }

    public sealed record Ping(Guid Id) : IRequest<Ping, byte[]>, IStreamRequest<Ping, byte[]>;

    public sealed class PingHandler : IRequestHandler<Ping, byte[]>
    {
        public ValueTask<byte[]> Handle(Ping request, CancellationToken cancellationToken)
        {
            var bytes = request.Id.ToByteArray();
            return new ValueTask<byte[]>(bytes);
        }
    }

    public class PingStreamHandler : IStreamRequestHandler<Ping, byte[]>
    {
        public async IAsyncEnumerable<byte[]> Handle(Ping request, CancellationToken cancellationToken)
        {
            yield break;
        }
    }
}
