using AuthScape.MAUI;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AuthScapeMAUI.PageModels
{
    public partial class LoginPageModel : ObservableObject
    {
        [ObservableProperty]
        private string loginToken;

        readonly AuthService _authService;
        readonly UserManagementService _userManagementService;
        public LoginPageModel(AuthService authService, UserManagementService userManagementService)
        {
            _authService = authService;
            _userManagementService = userManagementService;

            LoginToken = "Not logged in";
        }

        [RelayCommand]
        public async Task Authenticate()
        {
            await _authService.Authenticate();
        }

        //[RelayCommand]
        //public void Logout()
        //{
        //    _authService.Logout();
        //    LoginToken = "Not logged in";
        //}


        //[RelayCommand]
        //public async Task Appearing()
        //{
        //    var signedInUser = await _userManagementService.GetSignedInUser();
        //    if (signedInUser != null)
        //    {
        //        LoginToken = signedInUser.FirstName;
        //    }
        //}
    }
}