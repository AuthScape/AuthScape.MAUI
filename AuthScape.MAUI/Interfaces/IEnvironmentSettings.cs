namespace AuthScape.MAUI.Interfaces
{
    public interface IEnvironmentSettings
    {
        string DataScheme { get; }
        string ClientId { get; }
        string ClientSecret { get; }
        string BaseAPI { get; }
        string RedirectUri { get; }
        string BaseIDP { get; }
        string CompanyName { get; }
    }
}
