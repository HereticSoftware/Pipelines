
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
        services.AddScoped<global::Pipelines.Requests.IRequestHandler<global::Pipelines.Test.Integration.Ping, global::Pipelines.Test.Integration.Result<global::Pipelines.Test.Integration.Pong>>, global::Pipelines.Test.Integration.PingCommandHandler>();
        return services;
    }
}

#nullable disable
