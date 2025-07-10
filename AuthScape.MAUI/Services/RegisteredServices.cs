namespace AuthScape.MAUI.Services
{
    public class RegisteredServices
    {
        public static void Register(MauiAppBuilder builder)
        {

            builder.Services.AddSingleton<UserManagementService>();
            builder.Services.AddSingleton<AuthService>();
            builder.Services.AddSingleton<ApiService>();
            builder.Services.AddHttpClient<ApiService>();
        }
    }
}
