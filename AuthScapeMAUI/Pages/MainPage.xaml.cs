using AuthScape.MAUI.DeepLink;

namespace AuthScapeMAUI.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPageModel ViewModel { get; }

        public MainPage(MainPageModel viewModel)
        {
            InitializeComponent();
            BindingContext = ViewModel = viewModel;
        }
    }
}