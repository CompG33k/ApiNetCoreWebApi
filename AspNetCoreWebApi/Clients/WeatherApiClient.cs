using AspNetCoreWebApi.Clients.Interfaces;
using System.Net.Http.Headers;

namespace AspNetCoreWebApi.Clients
{
    public class WeatherApiClient   :   IApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<WeatherApiClient> _logger;
        string baseAddress = "https://api.openweathermap.org";
        private readonly string bearerToken = "BEAREAR_TOKEN";
        
        public WeatherApiClient(ILogger<WeatherApiClient>   logger,HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(baseAddress);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "AspNetCodeWebApi");
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/txt"));
        }

        public async Task<TResponse> GetAsync<TResponse>(string relativePath)
        {
            try
            {
                _logger.LogInformation("Sending GET request to {Url}", relativePath);
                var response = await _httpClient.GetAsync(relativePath);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadFromJsonAsync<TResponse>();

                if (result is null)
                {
                    throw new ApplicationException($"Deserialization returned null for {relativePath}");
                }
                return result!;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "An error occurred while calling the external API. Request URL: {Url}, Status Code: {StatusCode}",
                                 "{relativePath}", ex.StatusCode);
                throw new ApplicationException($"Failed to retrieve data from {relativePath}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during the API call to {Url}", relativePath);
                throw;
            }
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string relativePath, TRequest data)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(relativePath, data);
                response.EnsureSuccessStatusCode();
               var result = await response.Content.ReadFromJsonAsync<TResponse>();
                if (result is null)
                {
                    throw new ApplicationException($"Deserialization returned null for {relativePath}");
                }
                return result!;
            }
            catch(HttpRequestException ex)
            {
                _logger.LogError(ex, "An error occurred while calling the external API. Request URL: {Url}, Status Code: {StatusCode}",
                                 "{relativePath}", ex.StatusCode);
                throw new ApplicationException($"Failed to post data to {relativePath}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during the API call to {Url}", relativePath);
                throw;
            }
        }
    }
}