using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AuthScape.MAUI
{
    public class ApiService
    {
        private readonly HttpClient _client;
        private readonly string _baseUri = "http://localhost:54218/api";
        private bool _isRefreshing = false;

        public ApiService(HttpClient client)
        {
            _client = client;
            //_client.BaseAddress = new Uri(_baseUri);
        }

        private async Task<string> GetAccessTokenAsync()
        {
            return await SecureStorage.Default.GetAsync("access_token") ?? string.Empty;
        }

        private async Task<HttpRequestMessage> CreateRequestAsync(HttpMethod method, string url, HttpContent content = null)
        {
            var token = await GetAccessTokenAsync();
            var request = new HttpRequestMessage(method, url);
            if (!string.IsNullOrEmpty(token))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            if (content != null)
                request.Content = content;

            return request;
        }

        private async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            var response = await _client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.Unauthorized && !_isRefreshing)
            {
                _isRefreshing = true;
                var refreshed = await RefreshTokenAsync();
                _isRefreshing = false;

                if (refreshed)
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await GetAccessTokenAsync());
                    return await _client.SendAsync(request);
                }
            }
            return response;
        }

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            var request = await CreateRequestAsync(HttpMethod.Get, url);
            return await SendAsync(request);
        }

        public async Task<HttpResponseMessage> PostAsync(string url, HttpContent content)
        {
            var request = await CreateRequestAsync(HttpMethod.Post, url, content);
            return await SendAsync(request);
        }

        public async Task<HttpResponseMessage> PutAsync(string url, HttpContent content)
        {
            var request = await CreateRequestAsync(HttpMethod.Put, url, content);
            return await SendAsync(request);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string url)
        {
            var request = await CreateRequestAsync(HttpMethod.Delete, url);
            return await SendAsync(request);
        }

        private async Task<bool> RefreshTokenAsync()
        {
            var refreshToken = await SecureStorage.Default.GetAsync("refresh_token");
            if (string.IsNullOrEmpty(refreshToken)) return false;

            var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "grant_type", "refresh_token" },
            { "client_id", "your-client-id" },
            { "client_secret", "your-client-secret" },
            { "refresh_token", refreshToken }
        });

            var response = await _client.PostAsync("https://your-auth-server/connect/token", content);
            if (!response.IsSuccessStatusCode) return false;

            var json = await response.Content.ReadAsStringAsync();
            var tokenData = JsonSerializer.Deserialize<TokenResponse>(json);

            await SecureStorage.Default.SetAsync("access_token", tokenData.access_token);
            await SecureStorage.Default.SetAsync("refresh_token", tokenData.refresh_token);
            await SecureStorage.Default.SetAsync("expires_in", tokenData.expires_in.ToString());

            return true;
        }

        public async Task DownloadFileAsync(string url, string fileName, Action onComplete = null)
        {
            var request = await CreateRequestAsync(HttpMethod.Get, url);
            var response = await SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var bytes = await response.Content.ReadAsByteArrayAsync();
                var filePath = Path.Combine(FileSystem.CacheDirectory, fileName);
                File.WriteAllBytes(filePath, bytes);
                onComplete?.Invoke();
            }
        }

        private class TokenResponse
        {
            public string access_token { get; set; }
            public string refresh_token { get; set; }
            public int expires_in { get; set; }
        }
    }
}
