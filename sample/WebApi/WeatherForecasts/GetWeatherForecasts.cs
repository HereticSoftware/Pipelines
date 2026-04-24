using Pipelines.Requests;

namespace WebApi.WeatherForecasts;

public sealed record GetWeatherForecasts : IRequest<GetWeatherForecasts, WeatherForecast[]>;

public sealed class GetWeatherForecastsHandler : IRequestHandler<GetWeatherForecasts, WeatherForecast[]>
{
    private static readonly string[] Summaries =
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

    public ValueTask<WeatherForecast[]> Handle(GetWeatherForecasts request, CancellationToken cancellationToken = default)
    {
        var result = Enumerable
            .Range(1, 5)
            .Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)],
            })
            .ToArray();

        return new(result);
    }
}
