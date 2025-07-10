using Microsoft.UI.Xaml;
using Microsoft.Windows.AppLifecycle;
using Windows.ApplicationModel.Activation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AuthScapeMAUI.WinUI
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : MauiWinUIApplication
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

        protected override async void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            base.OnLaunched(args);

            var activationArgs = Microsoft.Windows.AppLifecycle.AppInstance.GetCurrent().GetActivatedEventArgs();

            if (activationArgs.Kind == ExtendedActivationKind.Protocol)
            {
                var protocolArgs = activationArgs.Data as IProtocolActivatedEventArgs;
                var uri = protocolArgs?.Uri;

                if (uri != null)
                {
                    string query = uri.Query; // e.g., ?user=123
                    string path = uri.AbsolutePath;

                    // Navigate or handle based on URI
                    await Shell.Current.GoToAsync($"{path}{query}");
                }
            }
        }
    }

}
