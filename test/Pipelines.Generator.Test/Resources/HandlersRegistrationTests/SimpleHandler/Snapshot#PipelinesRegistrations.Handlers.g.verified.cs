
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
        services.AddScoped<global::Pipelines.Requests.IRequestHandler<global::Some.Nested.Types.Ping, global::Some.Nested.Types.Pong>, global::Some.Nested.Types.PingHandler>();
        services.AddScoped<global::Pipelines.Streams.IStreamRequestHandler<global::Some.Nested.Types.Ping, global::Some.Nested.Types.Pong>, global::Some.Nested.Types.PingStreamHandler>();

        return services;
    }
}

#nullable disable
