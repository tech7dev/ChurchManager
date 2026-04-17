using ChurchMS.MAUI.Services;
using ChurchMS.MAUI.ViewModels;
using ChurchMS.MAUI.Views;
using Microsoft.Extensions.Logging;
using ZXing.Net.Maui.Controls;

namespace ChurchMS.MAUI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseBarcodeReader()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // HTTP client pointing to the deployed API
        builder.Services.AddHttpClient("ChurchMSApi", client =>
        {
            client.BaseAddress = new Uri("http://84.247.187.33:5009");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.Timeout = TimeSpan.FromSeconds(30);
        });

        // Core services
        builder.Services.AddSingleton<IAuthService, AuthService>();
        builder.Services.AddSingleton<ILocalDatabaseService, LocalDatabaseService>();
        builder.Services.AddSingleton<ApiService>();
        builder.Services.AddSingleton<SyncService>();

        // ViewModels
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<DashboardViewModel>();
        builder.Services.AddTransient<MembersViewModel>();
        builder.Services.AddTransient<GivingViewModel>();
        builder.Services.AddTransient<EventsViewModel>();

        // Views
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<DashboardPage>();
        builder.Services.AddTransient<MembersPage>();
        builder.Services.AddTransient<GivingPage>();
        builder.Services.AddTransient<EventsPage>();
        builder.Services.AddTransient<QrScannerPage>();

        return builder.Build();
    }
}
