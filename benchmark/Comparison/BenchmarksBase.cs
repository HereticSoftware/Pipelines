using DispatchR.Extensions;
using LiteBus.Extensions.Microsoft.DependencyInjection;
using LiteBus.Queries;
using Microsoft.Extensions.Hosting;

namespace Comparison;

[Config(typeof(BenchmarksConfig))]
public abstract class BenchmarksBase
{
    private readonly IServiceScope serviceScope;

    protected IHost Host { get; }

    /// <summary>
    /// The benchmark scoped service provider.
    /// </summary>
    protected IServiceProvider ServiceProvider => serviceScope.ServiceProvider;

    /// <summary>
    /// All initialized mediators/baseline handler from the benchmark service provider.
    /// </summary>
    protected MediatorServices Mediators { get; }

    protected BenchmarksBase()
    {
        var builder = Microsoft.Extensions.Hosting.Host.CreateEmptyApplicationBuilder(null);
        var services = builder.Services;

        services.AddLogging();

        // baseline
        services.AddScoped<HandlerBaseline>();

        // pipelines
        services.AddPipelines().AddHandlers();

        // mediatr
        services.AddMediatR(options =>
        {
            options.Lifetime = ServiceLifetime.Scoped;
            options.RegisterServicesFromAssembly(typeof(HandlerMediatR).Assembly);
        });

        // mediator
        services.AddMediator(options =>
        {
            options.ServiceLifetime = ServiceLifetime.Scoped;
        });

        // dispatchr
        services.AddDispatchR(options =>
        {
            options.Assemblies.Add(typeof(HandlerDispatchR).Assembly);
            options.RegisterPipelines = false;
            options.RegisterNotifications = false;
            OnGlobalSetupDispatchR(options);
        });

        // lite-bus
        services.AddLiteBus(options =>
        {
            options.AddQueryModule(module => module.RegisterFromAssembly(typeof(HandlerLiteBusQuery).Assembly));
            OnGlobalSetupLiteBus(options);
        });

        OnGlobalSetup(builder);
        Host = builder.Build();

        // setup services
        serviceScope = Host.Services.CreateScope();
        Mediators = new(ServiceProvider);

        OnGlobalSetup();
    }

    /// <summary>
    /// Allows derived classes to perform additional setup.
    /// </summary>
    protected virtual void OnGlobalSetup(IHostApplicationBuilder builder)
    {
    }

    /// <summary>
    /// Allows derived classes to perform additional setup for MediatR.
    /// </summary>
    protected virtual void OnGlobalSetupMediatR(MediatRServiceConfiguration options)
    {
    }

    /// <summary>
    /// Allows derived classes to perform additional setup for DispatchR.
    /// </summary>
    protected virtual void OnGlobalSetupDispatchR(DispatchR.Configuration.ConfigurationOptions options)
    {
    }

    /// <summary>
    /// Allows derived classes to perform additional setup for LiteBus.
    /// </summary>
    protected virtual void OnGlobalSetupLiteBus(LiteBus.Runtime.Abstractions.IModuleRegistry options)
    {
    }

    /// <summary>
    /// Allows derived classes to perform additional setup after the host is built.
    /// </summary>
    protected virtual void OnGlobalSetup()
    {
    }

    [GlobalCleanup]
    public void GlobalCleanup()
    {
        Host.Dispose();
    }

    protected readonly struct MediatorServices
    {
        public HandlerBaseline Baseline { get; }

        public Pipelines.Pipeline Pipeline { get; }

        public Pipelines.Requests.RequestPipeline<Request, Response> PipelineRequestRaw { get; }

        public Pipelines.Streams.StreamRequestPipeline<StreamRequest, Response> PipelineStreamRaw { get; }

        public MediatR.IMediator MediatrInterface { get; }

        public Mediator.IMediator MediatorInterface { get; }

        public Mediator.Mediator MediatorConcrete { get; }

        public DispatchR.IMediator DispatchR { get; }

        public LiteBus.Queries.Abstractions.IQueryMediator LiteBus { get; }

        public MediatorServices(IServiceProvider sp)
        {
            Baseline = sp.GetRequiredService<HandlerBaseline>();
            Pipeline = sp.GetRequiredService<Pipelines.Pipeline>();
            PipelineRequestRaw = sp.GetRequiredService<Pipelines.Requests.RequestPipeline<Request, Response>>();
            PipelineStreamRaw = sp.GetRequiredService<Pipelines.Streams.StreamRequestPipeline<StreamRequest, Response>>();
            MediatrInterface = sp.GetRequiredService<MediatR.IMediator>();
            MediatorInterface = sp.GetRequiredService<Mediator.IMediator>();
            MediatorConcrete = sp.GetRequiredService<Mediator.Mediator>();
            DispatchR = sp.GetRequiredService<DispatchR.IMediator>();
            LiteBus = sp.GetRequiredService<LiteBus.Queries.Abstractions.IQueryMediator>();
        }
    }
}
