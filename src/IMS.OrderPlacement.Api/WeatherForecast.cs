using Carter;

namespace IMS.OrderPlacement.Api;

public static class WeatherForecastService
{
    public class Endpoint : ICarterModule
    {
        readonly string[] _summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/weatherforecast", () =>
            {
#pragma warning disable CA5394 // Do not use insecure randomness
                var forecast = Enumerable.Range(1, 5).Select(index =>
                    new WeatherForecast
                    (
                        DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        Random.Shared.Next(-20, 55),
                        _summaries[Random.Shared.Next(_summaries.Length)]
                    ))
                    .ToArray();

                return forecast;
#pragma warning restore CA5394 // Do not use insecure randomness
            })
            .WithName("GetWeatherForecast");

        }
    }

    internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
    {
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}

