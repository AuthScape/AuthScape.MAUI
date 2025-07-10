using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AuthScape.MAUI
{
    public class AuthService
    {
        private string GenerateRandomString(int length)
        {
            var bytes = new byte[length];
            RandomNumberGenerator.Fill(bytes);
            return Convert.ToBase64String(bytes)
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');
        }

        private string GenerateCodeChallenge(string verifier)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.ASCII.GetBytes(verifier);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash)
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');
        }

        public async Task Authenticate()
        {
            string redirectUri = "authscape://open/mainpage"; // Must match your registered URI

            string state = "1234"; // Or generate dynamically
            string verifier = GenerateRandomString(64);
            string challenge = GenerateCodeChallenge(verifier);

            // Optionally store verifier for later token exchange
            await SecureStorage.Default.SetAsync("verifier", verifier);


            string authorityUri = "https://localhost:44303";
            string clientId = "postman";
            string scope = "email openid offline_access profile api1";
            string loginUri = $"{authorityUri}/connect/authorize" +
                $"?response_type=code" +
                $"&state={state}" +
                $"&client_id={clientId}" +
                $"&scope={Uri.EscapeDataString(scope)}" +
                $"&redirect_uri={Uri.EscapeDataString(redirectUri)}" +
                $"&code_challenge={challenge}" +
                $"&code_challenge_method=S256";

            await Launcher.Default.OpenAsync(new Uri(loginUri));
        }

    }
}
