using Foundation;
using UIKit;

namespace AuthScapeMAUI
{
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate
    {
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

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

        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            if (url.Scheme == "yourapp" && url.Host == "callback")
            {
                var parsed = ParseQueryParameters(url.AbsoluteString);
                var code = parsed["code"];

                // Pass it to your shared code
                return true;
            }

            return base.OpenUrl(app, url, options);
        }
    }
}
