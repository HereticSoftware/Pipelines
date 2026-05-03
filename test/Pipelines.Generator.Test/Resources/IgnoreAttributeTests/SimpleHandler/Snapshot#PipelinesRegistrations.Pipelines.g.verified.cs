
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
    public static global::Microsoft.Extensions.DependencyInjection.IServiceCollection AddPipelines(this global::Microsoft.Extensions.DependencyInjection.IServiceCollection services)
    {
        services.TryAddScoped(typeof(global::Pipelines.Requests.RequestPipeline<,>));
        services.TryAddScoped(typeof(global::Pipelines.Streams.StreamRequestPipeline<,>));
        services.TryAddScoped<global::Pipelines.Pipeline>();

        return services;
    }
}

#nullable disable
