using AuthScape.MAUI.Subscriptions;
using CommunityToolkit.Mvvm.Messaging;

namespace AuthScapeMAUI
{
    public partial class App : Application
    {
        public static IServiceProvider Services { get; private set; }

        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            Services = serviceProvider;


            WeakReferenceMessenger.Default.Register<LoggedOutMessage>(this, (r, m) =>
            {
                if (m.Value)
                {
                    var window = Application.Current?.Windows.FirstOrDefault();
                    if (window != null)
                    {
                        window.Page = Services.GetRequiredService<LoginPage>();

                        
                    }
                }
            });

            WeakReferenceMessenger.Default.Register<LoginMessage>(this, (r, m) =>
            {
                if (m.Value)
                {
                    var window = Application.Current?.Windows.FirstOrDefault();
                    if (window != null)
                    {
                        var appShellPage = Services.GetRequiredService<AppShell>();
                        window.Page = appShellPage;

                        if (Shell.Current.CurrentPage?.BindingContext is MainPageModel vm)
                        {
                            vm.AppearingCommand.Execute(null);
                        }
                    }
                }
            });
        }

        private bool IsLoggedIn()
        {
            var accessToken = SecureStorage.Default.GetAsync("access_token").Result;
            if (!String.IsNullOrWhiteSpace(accessToken))
            {
                return true;
            }
            return false;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            if (IsLoggedIn())
            {
                var appShellPage = Services.GetRequiredService<AppShell>();
                return new Window(appShellPage);
            }
            else
            {
                var loginPage = Services.GetRequiredService<LoginPage>();
                return new Window(loginPage);
            }
        }
    }
}