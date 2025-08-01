using AuthScape.MAUI.Interfaces;

namespace AuthScapeMAUI.Models
{
    public class EnvironmentSettings : IEnvironmentSettings
    {
        public bool IsDebug => EnvironmentConstants.IsDebug;
        public string DataScheme => EnvironmentConstants.DataScheme;
        public string ClientId => EnvironmentConstants.ClientId;
        public string ClientSecret => EnvironmentConstants.ClientSecret;
        public string BaseAPI => EnvironmentConstants.BaseAPI;
        public string RedirectUri => EnvironmentConstants.RedirectUri;
        public string BaseIDP => EnvironmentConstants.BaseIDP;
        public string CompanyName => EnvironmentConstants.CompanyName;
    }
}
