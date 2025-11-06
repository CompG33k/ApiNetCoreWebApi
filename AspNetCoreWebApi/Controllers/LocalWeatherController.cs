using AspNetCoreWebApi.Clients.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LocalWeatherController : ControllerBase
    {
        ILogger<LocalWeatherController> _logger;
        IApiClient _webApiClient;
        public LocalWeatherController(ILogger<LocalWeatherController> logger, IApiClient wApiClient)
        {
            _logger = logger;
            _webApiClient = wApiClient;
        }

        [HttpGet(Name = "GetLocalWeather")]
        public async Task<ActionResult> GetAsync()
        {
            var weather = await _webApiClient.GetAsync<IEnumerable<dynamic>>();

            if (weather == null || !weather.Any())
            {
                return NotFound("No products found."); // Returns 404 Not Found
            }

            return Ok(weather);
        }
    }
}
