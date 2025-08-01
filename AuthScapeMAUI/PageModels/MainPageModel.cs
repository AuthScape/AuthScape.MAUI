using AuthScape.MAUI;
using AuthScapeMAUI.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace AuthScapeMAUI.PageModels
{
    public partial class MainPageModel : ObservableObject
    {
        [ObservableProperty]
        private string loginToken;

        readonly AuthService _authService;
        readonly ApiService _apiService;
        readonly UserManagementService _userManagementService;
        public MainPageModel(AuthService authService, UserManagementService userManagementService, ApiService apiService)
        {
            _authService = authService;
            _userManagementService = userManagementService;
            _apiService = apiService;

            Task.Run(async () =>
            {
                await Appearing();
            });
        }

        [RelayCommand]
        public async Task Authenticate()
        {
            await _authService.Authenticate();
        }

        [RelayCommand]
        public void Logout()
        {
            _authService.Logout();
            LoginToken = "Not logged in";
        }


        [RelayCommand]
        public async Task Appearing()
        {
            var signedInUser = await _userManagementService.GetSignedInUser();
            if (signedInUser != null)
            {
                LoginToken = signedInUser.FirstName;
            }


            //var response = await _apiService.GetAsync("MobileApp/GetCustomers");

            //var stringResponse = await response.Content.ReadAsStringAsync();

            //var convertedJSON = await response.Content.ReadFromJsonAsync<List<Customer>>();
            //var Customers = new ObservableCollection<Customer>(convertedJSON ?? new List<Customer>());




            var response = await _apiService.GetAsync("MobileApp/GetLocations?companyId=" + 3);
            var convertedJSON = await response.Content.ReadFromJsonAsync<List<AuthScapeMAUI.Models.Location>>();
            var Locations = new ObservableCollection<AuthScapeMAUI.Models.Location>(convertedJSON ?? new List<AuthScapeMAUI.Models.Location>());


        }
    }
}