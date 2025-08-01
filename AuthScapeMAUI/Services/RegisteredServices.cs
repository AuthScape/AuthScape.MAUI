using AuthScape.MAUI;
using AuthScape.MAUI.Interfaces;
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

            builder.Services.AddHttpClient<ApiService>((provider, client) =>
            {
                var env = provider.GetRequiredService<IEnvironmentSettings>();

                var baseApi = env.BaseAPI;
                if (!baseApi.EndsWith("/"))
                    baseApi += "/";
                client.BaseAddress = new Uri(baseApi);
            });

        }
    }
}