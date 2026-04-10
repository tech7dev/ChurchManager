using ZXing.Net.Maui;

namespace ChurchMS.MAUI.Views;

public partial class QrScannerPage : ContentPage
{
    private bool _detected;

    public QrScannerPage()
    {
        InitializeComponent();
    }

    private void OnBarcodeDetected(object sender, BarcodeDetectionEventArgs e)
    {
        if (_detected) return;
        _detected = true;

        var result = e.Results.FirstOrDefault();
        if (result is null) return;

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            ResultLabel.Text = $"Detected: {result.Value}";
            BarcodeReader.IsDetecting = false;

            // Return the QR value to the calling page
            await Shell.Current.GoToAsync($"..",
                new Dictionary<string, object>
                {
                    ["qrValue"] = result.Value
                });
        });
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}
