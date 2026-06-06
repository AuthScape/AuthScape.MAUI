using AuthScape.MAUI.Subscriptions;
using CommunityToolkit.Mvvm.Messaging;
using System.Security.Cryptography;
using System.Text;

namespace AuthScape.MAUI
{
    public class AuthService
    {
        readonly string _authorityUri;
        readonly string _clientId;
        readonly string _redirectUri;

        public AuthService(string authorityUri, string clientId, string redirectUri)
        {
            _authorityUri = authorityUri;
            _clientId = clientId;
            _redirectUri = redirectUri;
        }

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
            // OIDC state must be cryptographically random per request and stashed so the
            // callback handler can verify the value bouncing back from the IDP matches what
            // we sent. A fixed value (the previous "1234") defeats CSRF protection — anyone
            // could craft a malicious /signin-oidc?code=...&state=1234 callback.
            string state = GenerateRandomString(32);
            string verifier = GenerateRandomString(64);
            string challenge = GenerateCodeChallenge(verifier);

            // Persist both for the token-exchange step. Verify state matches before
            // exchanging the code.
            await SecureStorage.Default.SetAsync("state", state);
            await SecureStorage.Default.SetAsync("verifier", verifier);

            string scope = "email openid offline_access profile api1";
            string loginUri = $"{_authorityUri}/connect/authorize" +
                $"?response_type=code" +
                $"&state={state}" +
                $"&client_id={_clientId}" +
                $"&scope={Uri.EscapeDataString(scope)}" +
                $"&redirect_uri={Uri.EscapeDataString(_redirectUri)}" +
                $"&code_challenge={challenge}" +
                $"&code_challenge_method=S256";

            await Launcher.Default.OpenAsync(new Uri(loginUri));
        }

        public void Logout()
        {
            // Clear stored tokens
            SecureStorage.Default.RemoveAll();

            WeakReferenceMessenger.Default.Send(new LoggedOutMessage(true));
        }
    }
}
