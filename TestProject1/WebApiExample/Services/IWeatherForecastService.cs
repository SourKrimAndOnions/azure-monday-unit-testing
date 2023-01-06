namespace WebApiExample.Services;

public interface IWeatherForecastService
{
    IEnumerable<WeatherForecast> GetWeatherForecasts();
};