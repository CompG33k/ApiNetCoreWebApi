using Microsoft.AspNetCore.Mvc;
using AspNetCoreWebApi.BusinessLayer.Interfaces;
using AspnetCoreWebApi.DataLayer.Entities;

namespace AspNetCoreWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private ISummaries _summaries;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, ISummaries sumaries)
        {
            _logger = logger;
            _summaries = sumaries;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = _summaries.GetSummaries()[Random.Shared.Next(_summaries.GetLength())]
            })
            .ToArray();
        }
    }
}
