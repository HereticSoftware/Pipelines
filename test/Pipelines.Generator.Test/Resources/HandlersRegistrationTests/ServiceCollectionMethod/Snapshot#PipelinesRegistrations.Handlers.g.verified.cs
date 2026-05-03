
#nullable enable

using global::System.CodeDom.Compiler;
using global::Microsoft.Extensions.DependencyInjection;
using global::Microsoft.Extensions.DependencyInjection.Extensions;
using global::Pipelines;
using global::Pipelines.Requests;
using global::Pipelines.Streams;

namespace Microsoft.Extensions.DependencyInjection;

internal static partial class PipelinesRegistrations
{
    public static global::Microsoft.Extensions.DependencyInjection.IServiceCollection AddHandlers(this global::Microsoft.Extensions.DependencyInjection.IServiceCollection services)
    {
        services.AddScoped<global::Pipelines.Requests.IRequestHandler<global::Some.Program.Ping, byte[]>, global::Some.Program.PingHandler>();
        services.AddScoped<global::Pipelines.Streams.IStreamRequestHandler<global::Some.Program.Ping, byte[]>, global::Some.Program.PingStreamHandler>();

        return services;
    }
}

#nullable disable
