using AuthScape.MAUI.Interfaces;
using AuthScape.MAUI.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AuthScape.MAUI.Auth
{
    public class AccessVerifier
    {
        HttpClient client;

        public AccessVerifier()
        {
            // TODO: remove this for production use!!!!
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };

            client = new HttpClient(handler);
        }


        public async Task ExchangeCodeForTokensAsync(string code, IEnvironmentSettings environmentSettings)
        {
            var verifier = await SecureStorage.Default.GetAsync("verifier");
            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(verifier))
                return;

            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "grant_type", "authorization_code" },
                { "code", code },
                { "redirect_uri", environmentSettings.RedirectUri },
                { "client_id", environmentSettings.ClientId },
                { "client_secret", environmentSettings.ClientSecret }, // Optional for public clients
                { "code_verifier", verifier }
            });

            var response = await client.PostAsync(environmentSettings.BaseIDP + "/connect/token", content);
            var json = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var tokenData = JsonSerializer.Deserialize<TokenResponse>(json);

                await SecureStorage.Default.SetAsync("access_token", tokenData.access_token);
                await SecureStorage.Default.SetAsync("refresh_token", tokenData.refresh_token);
                await SecureStorage.Default.SetAsync("expires_in", tokenData.expires_in.ToString());
            }
            else
            {
                Debug.WriteLine($"Token exchange failed: {json}");
            }
        }

        public async Task GetSignedInUser(IEnvironmentSettings environmentSettings)
        {
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await SecureStorage.Default.GetAsync("access_token"));

            var response = await client.GetAsync(environmentSettings.BaseAPI + "/UserManagement");
            if (response != null)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    await SecureStorage.Default.SetAsync("user", await response.Content.ReadAsStringAsync());
                }
            }
        }


        public class TokenResponse
        {
            public string access_token { get; set; }
            public string refresh_token { get; set; }
            public string token_type { get; set; }
            public int expires_in { get; set; }
        }
    }
}
