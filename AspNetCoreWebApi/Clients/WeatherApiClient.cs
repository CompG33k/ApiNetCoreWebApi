using AspNetCoreWebApi.Clients.Interfaces;
using MediatR;
using Microsoft.Build.Framework;
using System.Net.Http.Headers;

namespace AspNetCoreWebApi.Clients
{
    public class WeatherApiClient   :   IApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<WeatherApiClient> _logger;
        private readonly string _pathUrl = "/locations/v1/cities/search?q=San Francisco";
        private readonly string bearerToken = "BEARER_TOKEN";

        public WeatherApiClient(ILogger<WeatherApiClient>   logger,HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://dataservice.accuweather.com");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "MyApp");
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<TResponse> GetAsync<TResponse>()
        {
            var response = await _httpClient.GetAsync(_pathUrl);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TResponse>();
        }

        public Task<TResponse> GetAsync<TResponse>(string relativePath)
        {
            throw new NotImplementedException();
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string relativePath, TRequest data)
        {
            var response = await _httpClient.PostAsJsonAsync(relativePath, data);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TResponse>();
        }
    }
}