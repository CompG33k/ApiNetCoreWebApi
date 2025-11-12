using AspNetCoreWebApi.Clients.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LocalWeatherController : ControllerBase
    {
        private readonly string _requestUrl = "/data/2.5/weather?q=London,uk&APPID=20d30669b4bca6954f9841457d9a0a1c";
        IApiClient _webApiClient;
        ILogger<LocalWeatherController> _logger;

        public LocalWeatherController(ILogger<LocalWeatherController> logger, IApiClient wApiClient)
        {
            _logger = logger;
            _webApiClient = wApiClient;
        }

        [HttpGet(Name = "GetLocalWeather")]
        public async Task<ActionResult> GetAsync()
        {
            try
            {
                var response    =   await _webApiClient.GetAsync<dynamic>(_requestUrl);
                // Throws an HttpRequestException if the status code is not a success code (2xx)
                response.EnsureSuccessStatusCode();
                return Ok(response);
            }
            catch (HttpRequestException ex)
            {
                // Log the exception details and contextual information
                _logger.LogError(ex, "An error occurred while calling the external API. Request URL: {Url}, Status Code: {StatusCode}",
                                 "{_requestUrl}", ex.StatusCode);

                // Depending on your API's requirements, you can:
                // 1. Throw a new exception to be handled by a global exception handler or middleware.
                throw new ApplicationException($"Failed to retrieve data from {_requestUrl}", ex);

                // 2. Return a default value or an error result.
                // return null; 
            }
            catch (Exception ex)
            {
                // Handle any other unexpected exceptions
                _logger.LogError(ex, "An unexpected error occurred during the API call to {Url}", _requestUrl);
                throw; // Re-throw the exception to maintain flow
            }
        }
    }
}
