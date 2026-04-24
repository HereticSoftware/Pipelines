namespace Pipelines.Generator;

/// <summary>
/// Represents a set of features detected in the project, such as the presence of specific dependencies.
/// </summary>
public sealed record class Features
{
    /// <summary>
    /// Gets a value indicating whether the project has a reference to Microsoft.Extensions.DependencyInjection.
    /// </summary>
    public bool HasDependencyInjection { get; init; }

    /// <summary>
    /// Gets a value indicating whether all handlers should be registered automatically.
    /// </summary>
    public bool DisableHandlerRegistration { get; init; }

    /// <summary>
    /// Gets a value indicating whether the automatic mediator registration should be disabled.
    /// </summary>
    public bool DisableMediatorRegistration { get; init; }

    /// <summary>
    /// Create the features record from the <see cref="IncrementalGeneratorInitializationContext"/>.
    /// </summary>
    public static IncrementalValueProvider<Features> Create(in IncrementalGeneratorInitializationContext context)
    {
        var hasDependencyInjection = context
            .MetadataReferencesProvider
            .Where(static reference =>
                reference.Display?.EndsWith("Microsoft.Extensions.DependencyInjection.dll") is true
            )
            .Collect()
            .Select((metadata, ct) => !metadata.IsEmpty)
            .WithTrackingName("Features.HasDependencyInjection");

        var disableHandlerRegistration = context
            .AnalyzerConfigOptionsProvider
            .Select(static (config, _) =>
                config.GlobalOptions.TryGetValue("build_property.Pipelines_DisableHandlerRegistration", out var d) && d.Equals("true", StringComparison.OrdinalIgnoreCase)
            )
            .WithTrackingName("Features.DisableHandlerRegistration");

        var disableMediatorRegistration = context
            .AnalyzerConfigOptionsProvider
            .Select(static (config, _) =>
                config.GlobalOptions.TryGetValue("build_property.Pipelines_DisableMediatorRegistration", out var d) && d.Equals("true", StringComparison.OrdinalIgnoreCase)
            )
            .WithTrackingName("Features.DisableMediatorRegistration");

        var features = IncrementalValueProviderExtensions
            .Combine(hasDependencyInjection, disableHandlerRegistration)
            .Combine(disableMediatorRegistration)
            .Select((s, ct) => new Features
            {
                HasDependencyInjection = s.Left.Left,
                DisableHandlerRegistration = s.Left.Right,
                DisableMediatorRegistration = s.Right,
            })
            .WithTrackingName("Features");

        return features;
    }
}
