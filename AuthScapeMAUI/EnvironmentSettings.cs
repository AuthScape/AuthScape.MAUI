using AuthScape.MAUI;

namespace AuthScapeMAUI
{
    public class MyEnvironmentSettings : IEnvironmentSettings
    {
        public string ClientId { get; set; } = "postman";
        public string Secret { get; set; } = "";
        public string BaseAPI { get; set; } = "http://localhost:54218/api";
        public string RedirectUri { get; set; } = "authscape://open/mainpage";
        public string BaseIDP { get; set; } = "https://localhost:44303";
        public string CompanyName { get; set; } = "AuthScape";
    }
}