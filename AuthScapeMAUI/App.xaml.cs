using AuthScape.MAUI.DeepLink;

namespace AuthScapeMAUI
{
    public partial class App : Application
    {
        public static App? CurrentApp => Application.Current as App;

        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }

        protected override void OnAppLinkRequestReceived(Uri uri)
        {
            base.OnAppLinkRequestReceived(uri);
            LinkReceived.OnAppLinkRequestReceived(uri);
        }
    }
}