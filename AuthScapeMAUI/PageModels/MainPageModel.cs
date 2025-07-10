using AuthScape.MAUI;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AuthScapeMAUI.PageModels
{
    public partial class MainPageModel : ObservableObject
    {
        [ObservableProperty]
        private string loginToken;

        readonly AuthService _authService;
        readonly UserManagementService _userManagementService;
        public MainPageModel(AuthService authService, UserManagementService userManagementService)
        {
            _authService = authService;
            _userManagementService = userManagementService;
        }

        [RelayCommand]
        public async Task Authenticate()
        {
            await _authService.Authenticate();
        }

        [RelayCommand]
        public async void Appearing()
        {
            var signedInUser = await _userManagementService.GetSignedInUser();
            if (signedInUser != null)
            {
                LoginToken = signedInUser.FirstName;
            }
        }
    }
}