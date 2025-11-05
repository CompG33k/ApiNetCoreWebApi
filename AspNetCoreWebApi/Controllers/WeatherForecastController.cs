using Microsoft.AspNetCore.Mvc;
using AspNetCoreWebApi.BusinessLayer.Interfaces;
using AspnetCoreWebApi.DataLayer.Entities;
using AspNetCoreWebApi.Clients;

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
            _logger.LogInformation("This is an informational log message from MyController.");
            _logger.LogError("This is an error log message.");
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IActionResult Get()
        {
            _logger.LogInformation("Calling get Method");
            try
            {
                return Ok(Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = _summaries.GetSummaries()[Random.Shared.Next(_summaries.GetLength())]
                })
            .ToArray());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
