using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;

namespace AuraIDE.Services
{
    public class HttpService : IHttpService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HttpService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor) {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public TOutput Get<TOutput>(string uri, bool sendAccessToken) {
            using (var httpClient = _httpClientFactory.CreateClient()) {
                if (sendAccessToken) {
                    var accessToken = GetAccessToken().Result;
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                }
                var response = httpClient.GetAsync(uri).Result;
                return JsonConvert.DeserializeObject<TOutput>(response.Content.ReadAsStringAsync().Result);
            }
        }

        public async Task<TOutput> GetAsync<TOutput>(string uri, bool sendAccessToken = false) {
            using (var httpClient = _httpClientFactory.CreateClient()) {
                if (sendAccessToken) {
                    var accessToken = await GetAccessToken();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                }
                var response = await httpClient.GetAsync(uri);
                return JsonConvert.DeserializeObject<TOutput>(await response.Content.ReadAsStringAsync());
            }
        }

        public TOutput Post<TInput, TOutput>(string uri, TInput input, bool sendAccessToken = false) {
            using (var httpClient = _httpClientFactory.CreateClient()) {
                if (sendAccessToken) {
                    var accessToken = GetAccessToken().Result;
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                }
                JsonContent content = JsonContent.Create(input);
                var response = httpClient.PostAsync(uri, content).Result;
                return JsonConvert.DeserializeObject<TOutput>(response.Content.ReadAsStringAsync().Result);
            }
        }
        
        public async Task<TOutput> PostAsync<TInput, TOutput>(string uri, TInput input, bool sendAccessToken = false) {
            using (var httpClient = _httpClientFactory.CreateClient()) {
                if (sendAccessToken) {
                    var accessToken = await GetAccessToken();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                }
                JsonContent content = JsonContent.Create(input);
                var response = await httpClient.PostAsync(uri, content);
                return JsonConvert.DeserializeObject<TOutput>(await response.Content.ReadAsStringAsync());
            }
        }
        private async Task<string> GetAccessToken()
        {
            var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            if (accessToken != null) return accessToken;
            string authorizationHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            if(authorizationHeader != null)
            {
                string []values = authorizationHeader.Split(" ");
                if (values.Length > 1) return values[1];
            }
            return null;
        }
    }
}