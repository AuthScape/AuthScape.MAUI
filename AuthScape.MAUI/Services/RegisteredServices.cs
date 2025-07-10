using System.Text.Json;

namespace AuthScape.MAUI.Services
{
    public class RegisteredServices
    {
        public static void Register(MauiAppBuilder builder, IEnvironmentSettings environmentSettings)
        {
            builder.Services.AddSingleton(new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });

            builder.Services.AddSingleton<AuthService>(provider =>
            {
                var authorityUri = environmentSettings.BaseIDP; // Replace with your actual value
                var clientId = environmentSettings.ClientId; // Replace with your actual value
                var redirectUri = environmentSettings.RedirectUri; // Replace with your actual value
                
                return new AuthService(authorityUri, clientId, redirectUri);
            });

            builder.Services.AddSingleton<ApiService>(provider =>
            {
                var baseUri = environmentSettings.BaseAPI; // Replace with your actual value

                return new ApiService(baseUri);
            });

            builder.Services.AddHttpClient<ApiService>();
        }
    }
}