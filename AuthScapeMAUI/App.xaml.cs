using AuthScape.MAUI.DeepLink;
using AuthScapeMAUI.Models;

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

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                var settings = new EnvironmentSettings();
                await LinkReceived.OnAppLinkRequestReceived(uri, settings);
            });
        }
    }
}