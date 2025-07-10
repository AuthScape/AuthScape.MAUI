using AuthScape.MAUI;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AuthScapeMAUI.PageModels
{
    public partial class MainPageModel : ObservableObject
    {
        readonly AuthService _authService;
        public MainPageModel(AuthService authService)
        {
            _authService = authService;
        }

        [RelayCommand]
        public async Task Authenticate()
        {
            await _authService.Authenticate();
        }
    }
}