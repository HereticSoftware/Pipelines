# Benchmarks

Benchmark projects to benchmark pipelines:
* [Comparison](./Comparison/): Compares various mediator libraries to Pipelines.

## Running
To run all comparison benchmarks use the following command:

```bash
dotnet run -f net10.0 -c Release --project .\Comparison\ -- -f *
```

You can filter benchmarks using the command-line arguments as described [here](https://benchmarkdotnet.org/articles/guides/console-args.html).
