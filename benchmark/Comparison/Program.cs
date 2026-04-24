var switcher = BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly);
var summaries = switcher.Run(args);

foreach (var summary in summaries)
{
    if (Directory.Exists(summary.ResultsDirectoryPath) && File.Exists(summary.LogFilePath))
    {
        var name = Path.GetFileName(summary.LogFilePath);
        File.Move(summary.LogFilePath, Path.Combine(summary.ResultsDirectoryPath, name), true);
    }
}
