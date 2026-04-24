using Pipelines;
using Pipelines.Requests;
using Scalar.AspNetCore;
using WebApi.WeatherForecasts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddPipelines(); // add pipeline (mediator) services/types
builder.Services.AddHandlers(); // adds our handlers that have been defined in this assembly

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference("/docs");
}

app.UseHttpsRedirection();

app
    .MapGet("/weatherforecast", (Pipeline pipeline, CancellationToken ct) => pipeline.Request.Execute(new GetWeatherForecasts(), ct))
    .WithName("Get Weather Forecast")
    .WithDescription("Uses the Pipeline (mediator) type to send a request, since the request implements the IRequest interface the method only needs a new Instance of the request type.");

app
    .MapGet("/weatherforecast/direct", (RequestPipeline<GetWeatherForecasts, WeatherForecast[]> pipeline, CancellationToken ct) => pipeline.Execute(new(), ct))
    .WithName("Get Weather Forecast Direct")
    .WithDescription("Uses the RequestPipeline<TRequest, TResponse> type to send a request, this acceptes only request of TRequest, the combination can be anything so be carefull.");

app
    .MapGet("/weatherforecast/direct-wrong-type", (RequestPipeline<GetWeatherForecasts, object[]> pipeline, CancellationToken ct) => pipeline.Execute(new(), ct))
    .WithName("Get Weather Forecast Direct Wrong Return Type")
    .WithDescription("Uses the RequestPipeline<TRequest, TResponse> type to send a request, this acceptes only request of TRequest, with the wrong combination resulting in a Unable to resolve service for type error as this handler does not exist.");

await app.RunAsync();
