using AuthScape.MAUI;
using AuthScape.MAUI.Interfaces;

namespace AuthScapeMAUI
{
    public class EnvironmentConstants
    {
        public const bool IsDebug = true; // Set to false for production
        public const string DataScheme = "authscape";
        public const string ClientId = "postman";
        public const string ClientSecret = "postman-secret";

        // Use localhost for Windows, or your network IP for iOS/Android simulators
        private static string GetBaseUrl()
        {
#if WINDOWS
            return "localhost";
#else
            return "192.168.0.147"; // Your Windows machine IP on local network
#endif
        }

        public static string BaseAPI => $"http://{GetBaseUrl()}:54218/api";
        public static string BaseIDP => $"https://{GetBaseUrl()}:44303";

        public const string RedirectUri = "authscape://mainpage";
        public const string CompanyName = "AuthScape";
    }
}