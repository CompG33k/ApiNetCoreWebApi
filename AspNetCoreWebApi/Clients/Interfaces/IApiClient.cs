namespace AspNetCoreWebApi.Clients.Interfaces
{
    public interface IApiClient
    {
        Task<TResponse> GetAsync<TResponse>();
        Task<TResponse> GetAsync<TResponse>(string relativePath);
        Task<TResponse> PostAsync<TRequest, TResponse>(string relativePath, TRequest data);
    }
}
