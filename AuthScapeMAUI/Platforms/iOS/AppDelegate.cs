using Foundation;
using UIKit;

namespace AuthScapeMAUI
{
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate
    {
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            if (url.Scheme?.ToLower() == EnvironmentConstants.DataScheme.ToLower())
            {
                TryHandleDeepLink(url);
                return true;
            }

            return base.OpenUrl(app, url, options);
        }

        private void TryHandleDeepLink(NSUrl nsUrl)
        {
            try
            {
                var dotnetUri = new Uri(nsUrl.AbsoluteString);

                // Ensure MAUI is ready before processing
                Microsoft.Maui.ApplicationModel.MainThread.BeginInvokeOnMainThread(async () =>
                {
                    var appShell = App.Services.GetRequiredService<AppShell>();

                    var settings = new EnvironmentSettings();
                    await AuthScape.MAUI.DeepLink.LinkReceived.OnAppLinkRequestReceived(dotnetUri, settings, appShell);
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[DeepLinkError] {ex.Message}");
            }
        }
    }
}
