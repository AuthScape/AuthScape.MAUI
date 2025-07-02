using AuthScape.MAUI.Auth;

namespace AuthScape.MAUI.DeepLink
{
    public class LinkReceived
    {
        public void HandleAppLink(Uri uri)
        {
            OnAppLinkRequestReceived(uri);
        }

        public static async void OnAppLinkRequestReceived(Uri uri)
        {
            var route = uri.PathAndQuery.TrimStart('/');
            var routePath = route.Split('?')[0];
            var query = URI.ParseQueryString(uri.Query);

            if (Shell.Current?.CurrentItem?.CurrentItem is ShellSection section &&
                section.CurrentItem?.Content is Authentication authPage)
            {
                System.Diagnostics.Debug.WriteLine("Injecting query params into existing Authentication page.");
                authPage.ApplyQueryAttributes(query);
                return;
            }

            try
            {
                if (Shell.Current != null) // Ensure Shell.Current is not null before dereferencing
                {
                    await Shell.Current.GoToAsync(route);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("[Navigation Error]: Shell.Current is null.");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Navigation Error]: {ex.Message}");
            }
        }
    }
}
