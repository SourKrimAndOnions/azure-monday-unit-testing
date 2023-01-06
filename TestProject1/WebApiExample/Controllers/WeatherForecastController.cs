using Microsoft.AspNetCore.Mvc;
using WebApiExample.Services;

namespace WebApiExample.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly IWeatherForecastService forecastService;

    public WeatherForecastController(IWeatherForecastService forecastService)
    {
        this.forecastService = forecastService;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return forecastService.GetWeatherForecasts();
    }
}