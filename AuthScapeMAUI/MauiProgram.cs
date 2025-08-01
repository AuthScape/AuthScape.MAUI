using AuthScape.MAUI.Apps;
using AuthScape.MAUI.Interfaces;
using AuthScapeMAUI.Models;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Toolkit.Hosting;

namespace AuthScapeMAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureSyncfusionToolkit()
                .ConfigureMauiHandlers(handlers =>
                {
                })
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("SegoeUI-Semibold.ttf", "SegoeSemibold");
                    fonts.AddFont("FluentSystemIcons-Regular.ttf", FluentUI.FontFamily);
                });

#if DEBUG
    		builder.Logging.AddDebug();
    		builder.Services.AddLogging(configure => configure.AddDebug());
#endif

            builder.Services.AddSingleton<ModalErrorHandler>();
            builder.Services.AddSingleton<MainPageModel>();
            builder.Services.AddSingleton<LoginPageModel>();
            builder.Services.AddSingleton<LoginPage>();
            builder.Services.AddSingleton<AppShell>();



            RegisteredServices.Register(builder);

            builder.Services.AddSingleton<IEnvironmentSettings, EnvironmentSettings>();
            builder.Services.AddSingleton<UserManagementService>();
            

            // Setup for components that AuthScape Supports
            AuthScapeMauiBuilder.Build<App>(builder);


            return builder.Build();
        }
    }
}
