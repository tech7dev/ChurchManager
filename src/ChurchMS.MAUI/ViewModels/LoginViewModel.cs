using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ChurchMS.MAUI.Services;
using ChurchMS.MAUI.Views;

namespace ChurchMS.MAUI.ViewModels;

public partial class LoginViewModel(IAuthService authService) : BaseViewModel
{
    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [RelayCommand]
    private async Task LoginAsync()
    {
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            SetError("Please enter your email and password.");
            return;
        }

        IsBusy = true;
        ClearError();
        try
        {
            var success = await authService.LoginAsync(Email, Password);
            if (success)
            {
                await Shell.Current.GoToAsync("//dashboard");
            }
            else
            {
                SetError("Invalid credentials. Please try again.");
            }
        }
        catch (Exception ex)
        {
            SetError($"Login failed: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
