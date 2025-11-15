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

            // Subscribe to protocol activation for already-running app
            Microsoft.Windows.AppLifecycle.AppInstance.GetCurrent().Activated += OnActivated;
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

        protected override async void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            base.OnLaunched(args);

            // For unpackaged apps, check command-line arguments
            var commandLineArgs = Environment.GetCommandLineArgs();
            System.Diagnostics.Debug.WriteLine($"[Windows] Command line args: {string.Join(", ", commandLineArgs)}");

            if (commandLineArgs.Length > 1)
            {
                var uriString = commandLineArgs[1];
                System.Diagnostics.Debug.WriteLine($"[Windows] Checking arg: {uriString}");

                if (uriString.StartsWith("authscape://", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        var uri = new Uri(uriString);
                        await TryHandleDeepLink(uri);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"[Windows] Failed to parse URI: {ex.Message}");
                    }
                }
            }

            // Also try the packaged app method (in case it works)
            try
            {
                var activationArgs = Microsoft.Windows.AppLifecycle.AppInstance.GetCurrent().GetActivatedEventArgs();

                if (activationArgs?.Kind == ExtendedActivationKind.Protocol)
                {
                    var protocolArgs = activationArgs.Data as IProtocolActivatedEventArgs;
                    var uri = protocolArgs?.Uri;

                    if (uri != null && uri.Scheme?.ToLower() == EnvironmentConstants.DataScheme.ToLower())
                    {
                        await TryHandleDeepLink(uri);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Windows] Protocol activation check failed: {ex.Message}");
            }
        }

        private async void OnActivated(object sender, AppActivationArguments args)
        {
            // Handle protocol activation when app is already running
            System.Diagnostics.Debug.WriteLine($"[Windows] OnActivated called with kind: {args.Kind}");

            if (args.Kind == ExtendedActivationKind.Protocol)
            {
                var protocolArgs = args.Data as IProtocolActivatedEventArgs;
                var uri = protocolArgs?.Uri;

                System.Diagnostics.Debug.WriteLine($"[Windows] Protocol activation URI: {uri}");

                if (uri != null && uri.Scheme?.ToLower() == EnvironmentConstants.DataScheme.ToLower())
                {
                    await TryHandleDeepLink(uri);
                }
            }
        }

        private async Task TryHandleDeepLink(System.Uri uri)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"[Windows] TryHandleDeepLink called with URI: {uri}");

                // Wait a moment to ensure MAUI is fully initialized
                await Task.Delay(500);

                var appShell = AuthScapeMAUI.App.Services.GetRequiredService<AppShell>();
                var settings = new AuthScapeMAUI.Models.EnvironmentSettings();

                System.Diagnostics.Debug.WriteLine($"[Windows] Calling OnAppLinkRequestReceived");
                await AuthScape.MAUI.DeepLink.LinkReceived.OnAppLinkRequestReceived(uri, settings, appShell);
                System.Diagnostics.Debug.WriteLine($"[Windows] DeepLink handling completed");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[DeepLinkError] {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"[DeepLinkError] Stack: {ex.StackTrace}");
            }
        }
    }

}
