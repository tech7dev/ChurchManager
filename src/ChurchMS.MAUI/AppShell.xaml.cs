using ChurchMS.MAUI.Views;

namespace ChurchMS.MAUI;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute("qrscanner", typeof(QrScannerPage));
    }
}
