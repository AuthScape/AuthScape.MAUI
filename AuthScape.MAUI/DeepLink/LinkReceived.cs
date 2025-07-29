using AuthScape.MAUI.Auth;
using AuthScape.MAUI.Interfaces;
using AuthScape.MAUI.Subscriptions;
using CommunityToolkit.Mvvm.Messaging;

namespace AuthScape.MAUI.DeepLink
{
    public class LinkReceived
    {
        public static async Task OnAppLinkRequestReceived(Uri uri, IEnvironmentSettings environmentSettings, Page appShell)
        {
            var route = uri.PathAndQuery.TrimStart('/');
            var routePath = route.Split('?')[0];
            var query = URI.ParseQueryString(uri.Query);

            if (query.TryGetValue("code", out var tokenObj))
            {
                string token = Uri.UnescapeDataString(tokenObj?.ToString() ?? string.Empty);

                var accessVerifier = new AccessVerifier();

                await accessVerifier.ExchangeCodeForTokensAsync(token, environmentSettings); // Call the method to verify the token

                await accessVerifier.GetSignedInUser(environmentSettings);

                WeakReferenceMessenger.Default.Send(new LoginMessage(true));
            }
        }
    }
}
