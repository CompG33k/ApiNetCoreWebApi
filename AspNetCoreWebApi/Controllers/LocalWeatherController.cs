using AspnetCoreWebApi.DataLayer.Entities;
using AspNetCoreWebApi.BusinessLayer.Interfaces;
using AspNetCoreWebApi.Clients;
using AspNetCoreWebApi.Clients.Interfaces;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Text.Json.Nodes;

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
            _logger =   logger;
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

            return Ok( weather);
        }
    }
}
