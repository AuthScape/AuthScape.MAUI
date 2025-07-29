using AuthScape.MAUI.DeepLink;

namespace AuthScapeMAUI.Pages
{
    public partial class LoginPage : ContentPage
    {
        public LoginPageModel ViewModel { get; }

        public LoginPage(LoginPageModel viewModel)
        {
            InitializeComponent();
            BindingContext = ViewModel = viewModel;
        }
    }
}