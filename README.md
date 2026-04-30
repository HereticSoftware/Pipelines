# Pipelines

[![License](https://img.shields.io/github/license/HereticSoftware/Pipelines)](https://github.com/HereticSoftware/Pipelines/blob/main/LICENSE)
[![.NET Standard 2.0](https://img.shields.io/badge/.NET%20Standard%202.0-%23512bd4)](https://dotnet.microsoft.com/)
[![.NET 8](https://img.shields.io/badge/.NET%208-%23512bd4)](https://dotnet.microsoft.com/)
[![.NET 9](https://img.shields.io/badge/.NET%209-%23512bd4)](https://dotnet.microsoft.com/)
[![.NET 10](https://img.shields.io/badge/.NET%2010-%23512bd4)](https://dotnet.microsoft.com/)

[![NuGet Version](https://img.shields.io/nuget/v/Pipelines)](https://www.nuget.org/packages/Pipelines/)
[![NuGet Version](https://img.shields.io/nuget/vpre/Pipelines)](https://www.nuget.org/packages/Pipelines/)
[![Downloads](https://img.shields.io/nuget/dt/Pipelines)](https://www.nuget.org/packages/Pipelines/)

[![Publish](https://github.com/HereticSoftware/Pipelines/actions/workflows/publish.yaml/badge.svg)](https://github.com/HereticSoftware/Pipelines/actions/workflows/publish.yaml)

Pipelines is a lightweight library designed to simplify the implementation of the [mediator pattern](https://en.wikipedia.org/wiki/Mediator_pattern).

## Installation

Isntall the latest [package](https://www.nuget.org/packages/Pipelines) using the method that works for you as provided by the nuget site.

The package includes everything you need, including the generator.

## How It Works

1. **Request and Response**: Define your requests and responses.
1. **Handlers**: Create handlers for your request/response pairs.
1. **Execution**: Use the `Pipeline` class as a mediator to find the appropriate pipeline or inject/create a `RequestPipeline`, then execute the pipeline for a given request. This executes any behavrios before executing the handler and returns the expected response.
1. **Behaviors**: Add behaviors to your pipelines to customize the processing of requests and responses. This allows for additional functionality, such as logging, validation, or performance monitoring. Behavrios can be added to the DI too and will be injected.
1. **Source Generation**: The `Pipelines.Generator` simplifies the development process by generating code for handler registration, pipeline creation, and DI integration.

> [!NOTE]
> Implementing a specific interface for requests is optional, but it helps when using `Pipeline` class as you don't have to provide the generic arguments.

> [!NOTE]
> Generated handler registration registers handlers on their interfaces. Requesting an `IRequestHandler<object, object>` will give you your handler but requesting a `RequestPipeline<object, object>` will give you a built pipeline with your handler wrapped by any behaviors that could be injected.

## Examples

You can also find samples [here](/sample/).

### Requests

Declaring request/response/handler:

```csharp
public class MyRequest : IRequest<MyRequest, MyResponse> // interface here is optional
{
    public string Data { get; set; } = string.Empty;
}

public class MyResponse
{
    public string Result { get; set; } = string.Empty;
}

public class MyHandler : IRequestHandler<MyRequest, MyResponse>
{
    public ValueTask<MyResponse> Handle(MyRequest request)
    {
        return new(new MyResponse { Result = $"Processed: {request.Data}" });
    }
}
```
DI registration if using `Microsoft.Extensions.DependencyInjection`

```csharp
services.AddPipelines(); // adds pipelines to the di
services.AddHandlers(); // adds the assembly handlers to the di
```

Executing a request from di:

```csharp
var pipeline = serviceProvider.GetRequiredService<Pipeline>();

// with interface
var response1 = pipeline.Request(new MyRequest { Data = "Hello, World!" });

// without interface
var response2 = pipeline.Request<MyRequest, MyResponse>(new MyRequest { Data = "Hello, World!" });
```

### Streams

Declaring stream request/response/handler:

```csharp
public class MyStreamRequest : IStreamRequest<MyStreamRequest, string> // interface here is optional
{
    public string Query { get; set; }
}

public class MyStreamHandler : IStreamRequestHandler<MyStreamRequest, string>
{
    public async IAsyncEnumerable<string> Handle(MyStreamRequest request)
    {
        yield return $"Result 1 for {request.Query}";
        yield return $"Result 2 for {request.Query}";
    }
}
```
DI registration same as [Requests](#requests)

Executing a stream from di:

```csharp
var pipeline = serviceProvider.GetRequiredService<Pipeline>();

// with interface
await foreach (var result in pipeline.Stream(new MyStreamRequest { Query = "Test" }))
{
    Console.WriteLine(result);
}

// without interface
await foreach (var result in pipeline.Stream<MyStreamRequest, string>(new MyStreamRequest { Query = "Test" }))
{
    Console.WriteLine(result);
}
```

## Build and Test

To build and test the project you need an editor/ide of your choice and the [.NET SDK](https://dotnet.microsoft.com/en-us/download).

### Structure

#### Projects

- **Pipelines**: Provides the main contracts to build and execute a pipeline.
- **Pipelines.Generator**: Provides per assembly extension method generation for registering pipeline and handler implementations to the DI. Referenced by Pipelines and there is no need to install.

#### Features

- **Root**: Contains the `Pipeline` class that acts as a mediator, determining which pipeline to execute for a given request backed by an `IServiceProvider`.
- **Requests**: Request (Command, Query) functionality and contracts.
- **Streams**: StreamRequest functionality and contracts.

### Testing

The project uses [TUnit](https://tunit.dev) and the code was modeled after [Andrew Lock's](https://andrewlock.net) article [Creating a source generator](https://andrewlock.net/creating-a-source-generator-part-2-testing-an-incremental-generator-with-snapshot-testing/).

- **Pipelines.Generator.Test** Generator tests using [Verify](https://github.com/VerifyTests/Verify.SourceGenerators) to validated generated output for each possible case. Each test case has it's files in the [Resources](test/Pipelines.Generator.Test/Resources/) folder with a `Program.cs` file for compilation. it also includes the verify files. The naming convetion is `{ClassName}/{MethodName}/Program.cs`.
- **Pipelines.Generator.Test.Integration** Tests all pipeline classes and the generated code from the generator by trying to immitate day to day use.
- **Pipelines.Generator.Test.Integration.NuGet** Executes all integration tests by publishing the package at a local nuget source

All tests can be executed using dotnet (excluding nuget), visual studio (excluding nuget) or better by using [cake](https://cakebuild.net) eg:
- `dotnet cake.cs -c Release -- --target test`
- `dotnet cake.cs -c Release -- --target test-nuget`
