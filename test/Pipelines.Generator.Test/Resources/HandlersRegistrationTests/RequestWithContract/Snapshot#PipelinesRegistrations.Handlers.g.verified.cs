
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
        services.AddScoped<global::Pipelines.Requests.IRequestHandler<global::Pipelines.Test.Integration.InsideClass.Ping, global::Pipelines.Test.Integration.InsideClass.Pong>, global::Pipelines.Test.Integration.InsideClass.PingHandler>();
        services.AddScoped<global::Pipelines.Streams.IStreamRequestHandler<global::Pipelines.Test.Integration.InsideClass.Ping, global::Pipelines.Test.Integration.InsideClass.Pong>, global::Pipelines.Test.Integration.InsideClass.PingStreamHandler>();

        return services;
    }
}

#nullable disable
