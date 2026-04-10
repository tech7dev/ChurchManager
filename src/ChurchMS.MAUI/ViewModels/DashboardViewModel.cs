using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ChurchMS.Application.Features.Reports.DTOs;
using ChurchMS.MAUI.Services;

namespace ChurchMS.MAUI.ViewModels;

public partial class DashboardViewModel(
    ApiService apiService,
    IAuthService authService,
    SyncService syncService) : BaseViewModel
{
    [ObservableProperty]
    private DashboardSummaryDto? _dashboardData;

    [ObservableProperty]
    private string _greeting = string.Empty;

    [RelayCommand]
    public async Task LoadAsync()
    {
        Title = "Dashboard";
        Greeting = $"Welcome, {authService.GetUserName() ?? "User"}";
        IsBusy = true;
        ClearError();
        try
        {
            DashboardData = await apiService.GetAsync<DashboardSummaryDto>(
                "api/v1/reports/dashboard");

            // Sync in background
            _ = Task.Run(syncService.FullSyncAsync);
        }
        catch (Exception ex)
        {
            SetError($"Could not load dashboard: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task RefreshAsync() => await LoadAsync();

    [RelayCommand]
    private async Task LogoutAsync()
    {
        await authService.LogoutAsync();
        await Shell.Current.GoToAsync("//login");
    }
}
