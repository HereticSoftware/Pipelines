
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
        services.AddScoped<global::Pipelines.Requests.IRequestHandler<global::SomeProgram.Ping1, global::SomeProgram.Pong>, global::SomeProgram.Ping1Handler>();
        services.AddScoped<global::Pipelines.Requests.IRequestHandler<global::SomeProgram.Ping2, global::SomeProgram.Pong>, global::SomeProgram.Ping1Handler>();
        return services;
    }
}

#nullable disable
