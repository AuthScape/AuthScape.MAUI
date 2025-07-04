﻿using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using AuthScape.MAUI.DeepLink;

namespace AuthScapeMAUI
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    [IntentFilter(
        new[] { Intent.ActionView },
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
        DataScheme = "authscape", // your custom scheme
        DataHost = "open"         // optional
    )]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Cold start deep link
            if (Intent?.Data != null)
            {
                TryHandleDeepLink(Intent.Data);
            }
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);

            // Running instance receives deep link
            if (intent?.Data != null)
            {
                TryHandleDeepLink(intent.Data);
            }
        }

        private void TryHandleDeepLink(Android.Net.Uri uri)
        {
            try
            {
                var dotnetUri = new Uri(uri.ToString());

                // Wait a moment to ensure MAUI is ready (especially on cold start)
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    LinkReceived.OnAppLinkRequestReceived(dotnetUri);
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[DeepLinkError] {ex.Message}");
            }
        }

    }
}
