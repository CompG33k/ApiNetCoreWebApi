using AspNetCoreWebApi.Clients.Interfaces;
using System.Net.Http.Headers;

namespace AspNetCoreWebApi.Clients
{
    public class WeatherApiClient   :   IApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<WeatherApiClient> _logger;
        string baseAddress = "https://api.openweathermap.org";
        private readonly string _pathUrl = "/data/2.5/weather?q=London,uk&APPID=20d30669b4bca6954f9841457d9a0a1c";
        private readonly string bearerToken = "0d30669b4bca6954f9841457d9a0a1c";

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
            var response = await _httpClient.GetAsync(relativePath);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TResponse>();
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string relativePath, TRequest data)
        {
            var response = await _httpClient.PostAsJsonAsync(relativePath, data);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TResponse>();
        }
    }
}