using AuthScape.MAUI;
using System.Text.Json;

namespace AuthScapeMAUI.Services
{
    public class RegisteredServices
    {
        public static void Register(MauiAppBuilder builder)
        {
            builder.Services.AddSingleton(new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });

            builder.Services.AddSingleton(provider =>
            {
                var authorityUri = EnvironmentConstants.BaseIDP;
                var clientId = EnvironmentConstants.ClientId;
                var redirectUri = EnvironmentConstants.RedirectUri;
                
                return new AuthService(authorityUri, clientId, redirectUri);
            });

            builder.Services.AddSingleton(provider =>
            {
                var baseUri = EnvironmentConstants.BaseAPI;

                return new ApiService(baseUri);
            });

            builder.Services.AddHttpClient<ApiService>();
        }
    }
}