namespace AuraIDE.Services
{
    public interface IHttpService
    {
        Task<TOutput> GetAsync<TOutput>(string uri, bool sendAccessToken = false);
        Task<TOutput> PostAsync<TInput, TOutput>(string uri, TInput input, bool sendAccessToken = false);
        TOutput Get<TOutput>(string uri, bool sendAccessToken = false);
        TOutput Post<TInput, TOutput>(string uri, TInput input, bool sendAccessToken = false);
    }
}