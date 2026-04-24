using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using VerifyTests;
using VerifyTUnit;

namespace Pipelines.Generator.Test;

public abstract class TestBase
{
    [ModuleInitializer]
    public static void ModuleInitializer()
    {
        VerifySourceGenerators.Initialize();
    }

    public sealed class TestContext
    {
        private static readonly PortableExecutableReference[] DefaultReferences =
        [
            MetadataReference.CreateFromFile(typeof(Pipeline).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Microsoft.Extensions.DependencyInjection.ServiceProvider).Assembly.Location),
            .. AppDomain.CurrentDomain.GetAssemblies().Where(x => !x.IsDynamic).Select(x => MetadataReference.CreateFromFile(x.Location)),
        ];

        private static readonly string[] DefaultUsings =
        [
            "System",
            "System.Collections.Generic",
            "System.Runtime.CompilerServices",
            "System.Threading",
            "System.Threading.Tasks",
            "Microsoft.Extensions.DependencyInjection",
            "Pipelines",
            "Pipelines.Attributes",
            "Pipelines.Requests",
            "Pipelines.Streams",
        ];

        /// <summary>
        /// The name of the resource that will be tested.
        /// </summary>
        public string Resource { get; }

        /// <summary>
        /// The path to the resrouce file.
        /// </summary>
        public string ResourceFile { get; }

        /// <summary>
        /// The path to the snapshots directory.
        /// </summary>
        public string SnapshotDir { get; }

        /// <summary>
        /// Whether to ignore compilation errors or not.
        /// </summary>
        public bool IgnoreErrors { get; set; }

        /// <summary>
        /// References to include in the compilation.
        /// </summary>
        public List<PortableExecutableReference> References { get; }

        /// <summary>
        /// Usings to add at the top of the file after basic System and Pipelines namespaces.
        /// </summary>
        /// <remarks>
        /// The word <see langword="using"/> and the ending semicolon <see langword=";"/> is added automatically.
        /// </remarks>
        public List<string> Usings { get; }

        /// <summary>
        /// A function to determine whether a generated source result should be ignored.
        /// </summary>
        public Func<GeneratedSourceResult, bool> IgnoreGeneratedResult { get; set; } = _ => false;

        /// <summary>
        /// Create a new test context.
        /// </summary>
        public TestContext(Type testType, string resource)
        {
            Resource = resource;
            ResourceFile = Path.Combine(AppContext.BaseDirectory, "Resources", testType.Name, resource, "Program.cs");
            SnapshotDir = Path.Combine("Resources", testType.Name, resource);
            References = [.. DefaultReferences];
            Usings = [.. DefaultUsings];
        }

        /// <summary>
        /// Render usings to attach at the top of the code.
        /// </summary>
        public string RenderUsings()
        {
            return string.Join(Environment.NewLine, Usings.Select(u => $"using {u};"));
        }

        /// <summary>
        /// Assert that resource file and snapshots dir exist.
        /// </summary>
        public async Task That()
        {
            await Assert.That(File.Exists(ResourceFile)).IsTrue();
            await Assert.That(Directory.Exists(SnapshotDir)).IsTrue();
        }
    }

    protected async Task Verify(Action<TestContext>? configure = null, [CallerMemberName] string resource = null!)
    {
        var context = new TestContext(GetType(), resource);
        await context.That();

        configure?.Invoke(context);

        var source = await File.ReadAllTextAsync(context.ResourceFile);
        source = $"""
            {context.RenderUsings()}

            {source}
            """;

        var compilation = CSharpCompilation.Create(
            assemblyName: GetType().Name,
            syntaxTrees: [CSharpSyntaxTree.ParseText(source)],
            references: context.References,
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
        );

        var generator = new PipelinesGenerator().AsSourceGenerator();
        var driver = (GeneratorDriver)CSharpGeneratorDriver.Create([generator], driverOptions: new(default, true));
        driver = driver.RunGenerators(compilation);

        var runResult1 = driver.GetRunResult();
        var runResult2 = driver.RunGenerators(compilation.Clone()).GetRunResult();

        AssertNoErrors(runResult1.Diagnostics, context.IgnoreErrors);
        AssertNoErrors(runResult2.Diagnostics, context.IgnoreErrors);
        //AssertRunsEqual(runResult1, runResult2, trackingNames);

        await Verifier
            .Verify(driver)
            .ScrubLinesContaining("[InterceptsLocation(")
            .ScrubLines(l => l.StartsWith("//"))
            .ScrubLines(l => l.StartsWith("[GeneratedCodeAttribute"))
            .IgnoreGeneratedResult(context.IgnoreGeneratedResult)
            .UseDirectory(context.SnapshotDir)
            .UseFileName("Snapshot");
    }

    private static void AssertNoErrors(ImmutableArray<Diagnostic> diagnostics, bool skip)
    {
        if (skip) return;

        var errors = diagnostics.Where(x => x.Severity is DiagnosticSeverity.Error);
        if (errors.Any() is false)
            return;

        var texts = errors.Select(static x => x.ToString());
        var reason = string.Join(Environment.NewLine, texts);
        Assert.Fail(reason);
    }

    private static async Task AssertRunsEqual(GeneratorDriverRunResult runResult1, GeneratorDriverRunResult runResult2, string[] trackingNames)
    {
        // We're given all the tracking names, but not all the
        // stages will necessarily execute, so extract all the 
        // output steps, and filter to ones we know about
        var trackedSteps1 = GetTrackedSteps(runResult1, trackingNames);
        var trackedSteps2 = GetTrackedSteps(runResult2, trackingNames);

        // Both runs should have the same tracked steps

        await Assert.That(trackedSteps1).IsEquatableOrEqualTo(trackedSteps2);

        // Get the IncrementalGeneratorRunStep collection for each run
        foreach (var (trackingName, runSteps1) in trackedSteps1)
        {
            // Assert that both runs produced the same outputs
            var runSteps2 = trackedSteps2[trackingName];
            await Assert.That(runSteps1).IsEquatableOrEqualTo(runSteps2).Because(trackingName);
        }

        // Local function that extracts the tracked steps
        static Dictionary<string, ImmutableArray<IncrementalGeneratorRunStep>> GetTrackedSteps(
            GeneratorDriverRunResult runResult, string[] trackingNames)
            => runResult
                    .Results[0] // We're only running a single generator, so this is safe
                    .TrackedSteps // Get the pipeline outputs
                    .Where(step => trackingNames.Contains(step.Key)) // filter to known steps
                    .ToDictionary(x => x.Key, x => x.Value); // Convert to a dictionary
    }
}
