using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

namespace AuthScapeMAUI
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]

    [IntentFilter(
        new[] { Intent.ActionView },
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
        DataScheme = "yourapp",
        DataHost = "callback"
    )]

    public class MainActivity : MauiAppCompatActivity
    {
        public static IDictionary<string, string> ParseQueryParameters(string uri)
        {
            var result = new Dictionary<string, string>();
            var query = new Uri(uri).Query;

            if (query.StartsWith("?"))
                query = query.Substring(1);

            foreach (var pair in query.Split('&', StringSplitOptions.RemoveEmptyEntries))
            {
                var parts = pair.Split('=', 2);
                if (parts.Length == 2)
                {
                    var key = Uri.UnescapeDataString(parts[0]);
                    var value = Uri.UnescapeDataString(parts[1]);
                    result[key] = value;
                }
            }

            return result;
        }

        protected override void OnNewIntent(Intent? intent)
        {
            base.OnNewIntent(intent);

            var uri = intent?.Data?.ToString();
            if (!string.IsNullOrEmpty(uri) && uri.StartsWith("yourapp://callback"))
            {
                // Extract query parameters from the URI
                var parsed = ParseQueryParameters(uri);
                var code = parsed["code"]; // or whatever parameter you're expecting

                // Pass it to your shared code (e.g., via MessagingCenter, static service, etc.)
            }
        }
    }
}
