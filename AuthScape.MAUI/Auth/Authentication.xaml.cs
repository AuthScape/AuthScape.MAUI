namespace AuthScape.MAUI.Auth;

public partial class Authentication : ContentPage, IQueryAttributable
{
    private readonly ApiService _apiService;
    public Authentication(ApiService apiService)
    {
        _apiService = apiService;
        InitializeComponent();
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("code", out var tokenObj))
        {
            string token = Uri.UnescapeDataString(tokenObj?.ToString() ?? string.Empty);

            var accessVerifier = new AccessVerifier();
            await accessVerifier.ExchangeCodeForTokensAsync(token); // Call the method to verify the token

            var accessToken = await SecureStorage.Default.GetAsync("access_token");
            //Token.Text = accessToken;

            var userManagement = await _apiService.GetAsync("http://localhost:54218/api/UserManagement");
            if (userManagement != null)
            {
                if (userManagement.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    await SecureStorage.Default.SetAsync("user", await userManagement.Content.ReadAsStringAsync());

                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    //Token.Text = await userManagement.Content.ReadAsStringAsync();
                }
            }
        }
    }
}