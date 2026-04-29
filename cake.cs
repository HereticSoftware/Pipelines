#:sdk Cake.Sdk
#:property ManagePackageVersionsCentrally=false
#:package Cake.GitVersioning@3.9.50
#:package NuGet.Protocol@7.3.1

var target = Argument<string>("target");
var nugetApiKey = EnvironmentVariable("NUGET_API_KEY", string.Empty);
var nugetSource = EnvironmentVariable("NUGET_SOURCE", string.Empty);

var configuration = "Release";
var version = GitVersioningGetVersion();
Information("Version: {0}, Configuration: {1}", version.SemVer2, configuration);

DirectoryPath[] srcProjects = [
    "src/Pipelines",
];
DirectoryPath[] testProjects = [
    "test/Pipelines.Generator.Test",
    "test/Pipelines.Generator.Test.Integration",
];
DirectoryPath[] projects = [
    .. srcProjects,
    .. testProjects
];

var restore = Task("Restore")
    .DoesForEach(projects, dir =>
    {
        Information("\nRestore {0}", dir.GetDirectoryName());
        DotNetRestore(dir.FullPath);
    });

var build = Task("Build")
    .DoesForEach(projects, dir =>
    {
        Information("\nBuild {0}", dir.GetDirectoryName());
        DotNetBuild(dir.FullPath, new() { Configuration = configuration });
    });

var pack = Task("Pack")
    .IsDependentOn(build)
    .Does(() => CleanDirectory("packages"))
    .DoesForEach(srcProjects, file =>
        DotNetPack(file.FullPath, new() { NoRestore = true, NoBuild = true })
    );

var test = Task("Test")
    .DoesForEach(testProjects, dir =>
    {
        Information("\nTest {0}", dir.GetDirectoryName());
        DotNetTest(dir.FullPath, new() { PathType = DotNetTestPathType.Project, Configuration = configuration });
    });

var testNuget = Task("Test-NuGet")
    .IsDependentOn(pack)
    .Does(() =>
    {
        DirectoryPath dir = "test/Pipelines.Generator.Test.Integration.NuGet";
        DirectoryPath packagesDir = $"{dir}/packages";
        DirectoryPath pipelinesDir = $"{packagesDir}/pipelines";
        EnsureDirectoryExists(packagesDir);
        EnsureDirectoryExists(pipelinesDir);

        DeleteDirectory(pipelinesDir, new() { Recursive = true });
        DeleteFiles($"{packagesDir}/Pipelines**");

        Information("Copy {0} to {1}", "packages/*.nupkg", packagesDir.FullPath);
        CopyFiles("packages/*.nupkg", packagesDir);

        Information("Restore {0}", dir.GetDirectoryName());
        DotNetRestore(dir.FullPath, new() { PackagesDirectory = packagesDir, ConfigFile = $"{dir}/nuget.integration.config" });

        Information("Build {0}", dir.GetDirectoryName());
        DotNetBuild(dir.FullPath, new() { NoRestore = true, Configuration = configuration });

        Information("Test {0}", dir.GetDirectoryName());
        DotNetTest(dir.FullPath, new() { PathType = DotNetTestPathType.Project, NoBuild = true, NoRestore = true, Configuration = configuration });
    });

var pullRequest = Task("Pull-Request")
    .IsDependentOn(build)
    .IsDependentOn(test)
    .IsDependentOn(testNuget);

var publish = Task("Publish")
    .WithCriteria(!string.IsNullOrEmpty(nugetSource), "Environment variable `NUGET_API_KEY` was not provided")
    .WithCriteria(!string.IsNullOrEmpty(nugetApiKey), "Environment variable `NUGET_SOURCE` was not provided")
    .IsDependentOn(build)
    .IsDependentOn(test)
    .IsDependentOn(testNuget)
    .IsDependentOn(pack)
    .WithCriteria(() => GetFiles("packages/*.nupkg").Count != 0, "No packages were produced")
    .Does(() =>
    {
        Information("NuGet Push");
        DotNetNuGetPush("packages/*.nupkg", new() { Source = nugetSource, ApiKey = nugetApiKey });
    });

RunTarget(target);
