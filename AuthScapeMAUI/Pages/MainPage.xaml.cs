using AuthScapeMAUI.Models;
using AuthScapeMAUI.PageModels;

namespace AuthScapeMAUI.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }
    }
}