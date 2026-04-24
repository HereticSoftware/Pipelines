using Pipelines.Generator;
using Pipelines.Generator.Generators.PipelinesRegistration;
using Pipelines.Generator.Generators.HandlesrRegistration;

namespace Pipelines;

[Generator]
public sealed partial class PipelinesGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext initializationContext)
    {
        var context = new PipelinesGeneratorContext(initializationContext);

        HandlesrRegistrationGenerator.Initialize(context);
        PipelinesRegistrationGenerator.Initialize(context);
    }
}

internal sealed class PipelinesGeneratorContext
{
    public IncrementalGeneratorInitializationContext Context { get; }

    public IncrementalValueProvider<Features> Features { get; }

    public PipelinesGeneratorContext(in IncrementalGeneratorInitializationContext context)
    {
        Context = context;
        Features = Generator.Features.Create(context);
    }
}
