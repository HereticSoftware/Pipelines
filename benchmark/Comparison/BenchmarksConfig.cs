using BenchmarkDotNet.Exporters.Json;

namespace Comparison;

public sealed class BenchmarksConfig : ManualConfig
{
    public BenchmarksConfig()
    {
        AddJob([
            Job.Default.WithRuntime(CoreRuntime.Core80).WithId(".NET 8"),
            Job.Default.WithRuntime(CoreRuntime.Core90).WithId(".NET 9"),
            Job.Default.WithRuntime(CoreRuntime.Core10_0).WithId(".NET 10"),
        ]);

        AddLogger(ConsoleLogger.Default);
        AddDiagnoser(MemoryDiagnoser.Default);

        AddExporter(JsonExporter.Default);

        AddLogicalGroupRules(BenchmarkLogicalGroupRule.ByCategory);
        AddColumnProvider(DefaultColumnProviders.Instance);
        AddColumn(CategoriesColumn.Default, RankColumn.Arabic);
        HideColumns(Column.Arguments, Column.EnvironmentVariables, Column.BuildConfiguration, Column.Job);

        WithOption(ConfigOptions.KeepBenchmarkFiles, false);
        WithOption(ConfigOptions.JoinSummary, true);
        WithOption(ConfigOptions.DontOverwriteResults, true);
        WithSummaryStyle(SummaryStyle.Default.WithRatioStyle(RatioStyle.Percentage));
        WithOrderer(new DefaultOrderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared));
    }
}
