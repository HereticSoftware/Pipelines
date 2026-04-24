using System.Runtime.CompilerServices;
using Pipelines.Streams;

namespace Blazor.Components.Pages;

internal sealed record GetWeatherForecasts(int Count) : IStreamRequest<GetWeatherForecasts, WeatherForecast>;

internal sealed class WeatherForecast
{
    public DateOnly Date { get; set; }

    public int TemperatureC { get; set; }

    public string? Summary { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

internal sealed class GetWeatherForecastHandler : IStreamRequestHandler<GetWeatherForecasts, WeatherForecast>
{
    private static readonly string[] summaries =
    [
        "Freezing",
        "Bracing",
        "Chilly",
        "Cool",
        "Mild",
        "Warm",
        "Balmy",
        "Hot",
        "Sweltering",
        "Scorching",
    ];

    public async IAsyncEnumerable<WeatherForecast> Handle(GetWeatherForecasts query, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var startDate = DateOnly.FromDateTime(DateTime.Now);

        for (var i = 0; i < query.Count; i++)
        {
            await Task.Delay(300, cancellationToken);

            yield return new WeatherForecast
            {
                Date = startDate.AddDays(i),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = summaries[Random.Shared.Next(summaries.Length)],
            };
        }
    }
}
