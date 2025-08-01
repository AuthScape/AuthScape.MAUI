using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AuthScape.MAUI.Interfaces;
using Microsoft.Maui.Storage;

namespace AuthScape.MAUI
{
    public class ApiService
    {
        private readonly HttpClient _client;
        private readonly SemaphoreSlim _refreshLock = new SemaphoreSlim(1, 1);
        private readonly IEnvironmentSettings _environmentSettings;

        public ApiService(HttpClient client, IEnvironmentSettings environmentSettings)
        {
            _client = client;
            _environmentSettings = environmentSettings;
        }

        private async Task<string> GetAccessTokenAsync()
            => await SecureStorage.Default.GetAsync("access_token") ?? string.Empty;

        private async Task<DateTime> GetTokenExpiryAsync()
        {
            var expiresAtString = await SecureStorage.Default.GetAsync("expires_at");
            if (DateTime.TryParse(
                    expiresAtString,
                    null,
                    DateTimeStyles.RoundtripKind,
                    out var expiresAt))
            {
                return expiresAt;
            }
            return DateTime.MinValue;
        }

        private async Task EnsureValidTokenAsync()
        {
            // If token isn’t expiring within the next hour, nothing to do
            var expiresAt = await GetTokenExpiryAsync();
            if (DateTime.UtcNow.AddMinutes(15) < expiresAt)
                return;

            // Only one thread at a time may refresh
            await _refreshLock.WaitAsync();
            try
            {
                await RefreshTokenAsync();
            }
            finally
            {
                _refreshLock.Release();
            }
        }

        private async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            // Make sure our token is fresh
            await EnsureValidTokenAsync();

            var token = await GetAccessTokenAsync();
            if (!string.IsNullOrEmpty(token))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.SendAsync(request);

            // Fallback: if we still get 401, try one more time
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await _refreshLock.WaitAsync();
                try
                {
                    var refreshed = await RefreshTokenAsync();
                    if (refreshed)
                    {
                        token = await GetAccessTokenAsync();
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        response = await _client.SendAsync(request);
                    }
                }
                finally
                {
                    _refreshLock.Release();
                }
            }

            return response;
        }

        private async Task<HttpRequestMessage> CreateRequestAsync(HttpMethod method, string url, HttpContent content = null)
        {
            var request = new HttpRequestMessage(method, url);
            if (content != null)
                request.Content = content;
            return request;
        }

        public Task<HttpResponseMessage> GetAsync(string url)
            => CreateRequestAsync(HttpMethod.Get, url).ContinueWith(t => SendAsync(t.Result)).Unwrap();

        public Task<HttpResponseMessage> PostAsync(string url, HttpContent content)
            => CreateRequestAsync(HttpMethod.Post, url, content).ContinueWith(t => SendAsync(t.Result)).Unwrap();

        public Task<HttpResponseMessage> PutAsync(string url, HttpContent content)
            => CreateRequestAsync(HttpMethod.Put, url, content).ContinueWith(t => SendAsync(t.Result)).Unwrap();

        public Task<HttpResponseMessage> DeleteAsync(string url)
            => CreateRequestAsync(HttpMethod.Delete, url).ContinueWith(t => SendAsync(t.Result)).Unwrap();

        private async Task<bool> RefreshTokenAsync()
        {
            var refreshToken = await SecureStorage.Default.GetAsync("refresh_token");
            if (string.IsNullOrEmpty(refreshToken))
                return false;

            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "grant_type",    "refresh_token" },
                { "client_id",     _environmentSettings.ClientId },
                { "client_secret", _environmentSettings.ClientSecret },
                { "refresh_token", refreshToken }
            });

            // Hit your token endpoint
            HttpClient client;
            if (_environmentSettings.IsDebug)
            {
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                };

                client = new HttpClient(handler);
            }
            else
            {
                client = new HttpClient();
            }


            using (client)
            {
                var response = await client.PostAsync(_environmentSettings.BaseIDP + "/connect/token", content);
                if (!response.IsSuccessStatusCode)
                    return false;

                var json = await response.Content.ReadAsStringAsync();
                var tokenData = JsonSerializer.Deserialize<TokenResponse>(json);

                // Store new tokens...
                await SecureStorage.Default.SetAsync("access_token", tokenData.access_token);
                await SecureStorage.Default.SetAsync("refresh_token", tokenData.refresh_token);

                // ...and compute & store the exact expiry moment
                var expiresAt = DateTime.UtcNow.AddSeconds(tokenData.expires_in);
                await SecureStorage.Default.SetAsync("expires_at", expiresAt.ToString("o"));

                return true;
            }
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
