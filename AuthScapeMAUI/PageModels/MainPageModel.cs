using AuthScape.MAUI;
using AuthScape.MAUI.Services;
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
            LoginToken = await _userManagementService.GetSignedInUser(); // Use the generated property instead of directly referencing the field
        }
    }
}