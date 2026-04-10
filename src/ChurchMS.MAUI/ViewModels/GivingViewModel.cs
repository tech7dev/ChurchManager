using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ChurchMS.Application.Features.Contributions.DTOs;
using ChurchMS.MAUI.Models;
using ChurchMS.MAUI.Services;

namespace ChurchMS.MAUI.ViewModels;

public partial class GivingViewModel(
    ApiService apiService,
    ILocalDatabaseService localDb) : BaseViewModel
{
    [ObservableProperty]
    private ObservableCollection<ContributionListDto> _recentContributions = [];

    [ObservableProperty]
    private ObservableCollection<FundDto> _funds = [];

    // New contribution form
    [ObservableProperty] private decimal _amount;
    [ObservableProperty] private FundDto? _selectedFund;
    [ObservableProperty] private string _type = "Cash";
    [ObservableProperty] private string _notes = string.Empty;

    [RelayCommand]
    public async Task LoadAsync()
    {
        Title = "Giving";
        IsBusy = true;
        ClearError();
        try
        {
            var fundsResult = await apiService.GetPagedAsync<FundDto>(
                "api/v1/contributions/funds?page=1&pageSize=50");
            if (fundsResult?.Items is not null)
                Funds = new ObservableCollection<FundDto>(fundsResult.Items);

            var contribResult = await apiService.GetPagedAsync<ContributionListDto>(
                "api/v1/contributions?page=1&pageSize=10");
            if (contribResult?.Items is not null)
                RecentContributions = new ObservableCollection<ContributionListDto>(contribResult.Items);
        }
        catch (Exception ex)
        {
            SetError($"Could not load giving data: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task SubmitGivingAsync()
    {
        if (Amount <= 0)
        {
            SetError("Please enter a valid amount.");
            return;
        }
        if (SelectedFund is null)
        {
            SetError("Please select a fund.");
            return;
        }

        IsBusy = true;
        ClearError();
        try
        {
            var success = await apiService.PostAsync("api/v1/contributions", new
            {
                amount = Amount,
                fundId = SelectedFund.Id,
                type = Type,
                notes = Notes,
                contributionDate = DateOnly.FromDateTime(DateTime.Today)
            });

            if (success)
            {
                Amount = 0;
                Notes = string.Empty;
                await Application.Current!.MainPage!.DisplayAlert(
                    "Success", "Contribution recorded successfully!", "OK");
                await LoadAsync();
            }
            else
            {
                SetError("Failed to record contribution. It will sync when online.");
            }
        }
        catch (Exception ex)
        {
            SetError($"Error: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
