using AuthScape.MAUI;
using AuthScape.MAUI.Interfaces;

namespace AuthScapeMAUI
{
    public class EnvironmentConstants 
    {
        public const string DataScheme = "authscape";
        public const string ClientId = "postman";
        public const string ClientSecret = "postman-secret";
        public const string BaseAPI = "http://localhost:54218/api";
        public const string RedirectUri = "authscape://mainpage";
        public const string BaseIDP = "https://localhost:44303";
        public const string CompanyName = "AuthScape";
    }
}