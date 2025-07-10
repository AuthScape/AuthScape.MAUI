namespace AuthScape.MAUI
{
    public interface IEnvironmentSettings
    {
        public string ClientId { get; set; }
        public string Secret { get; set; }
        public string BaseAPI { get; set; }
        public string RedirectUri { get; set; }
        public string BaseIDP { get; set; }
        public string CompanyName { get; set; }
    }
}