using CommunityToolkit.Mvvm.ComponentModel;

namespace ChurchMS.MAUI.ViewModels;

public abstract partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    private bool _isBusy;

    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty]
    private string? _errorMessage;

    public bool IsNotBusy => !IsBusy;

    protected void SetError(string? message)
    {
        ErrorMessage = message;
    }

    protected void ClearError() => ErrorMessage = null;
}
