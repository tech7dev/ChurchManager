using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ChurchMS.MAUI.Models;
using ChurchMS.MAUI.Services;

namespace ChurchMS.MAUI.ViewModels;

public partial class MembersViewModel(
    ILocalDatabaseService localDb,
    SyncService syncService) : BaseViewModel
{
    [ObservableProperty]
    private ObservableCollection<LocalMember> _members = [];

    [ObservableProperty]
    private ObservableCollection<LocalMember> _filteredMembers = [];

    [ObservableProperty]
    private string _searchText = string.Empty;

    partial void OnSearchTextChanged(string value) => FilterMembers();

    [RelayCommand]
    public async Task LoadAsync()
    {
        Title = "Members";
        IsBusy = true;
        ClearError();
        try
        {
            // Sync from API if connected
            await syncService.SyncMembersAsync();

            var list = await localDb.GetMembersAsync();
            Members = new ObservableCollection<LocalMember>(list);
            FilterMembers();
        }
        catch (Exception ex)
        {
            SetError($"Could not load members: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        SearchText = string.Empty;
        await LoadAsync();
    }

    [RelayCommand]
    private async Task SelectMemberAsync(LocalMember member)
    {
        await Shell.Current.GoToAsync($"memberdetail?id={member.Id}");
    }

    private void FilterMembers()
    {
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            FilteredMembers = new ObservableCollection<LocalMember>(Members);
            return;
        }
        var term = SearchText.ToLowerInvariant();
        FilteredMembers = new ObservableCollection<LocalMember>(
            Members.Where(m =>
                m.FullName.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                m.Phone?.Contains(term, StringComparison.OrdinalIgnoreCase) == true ||
                m.Email?.Contains(term, StringComparison.OrdinalIgnoreCase) == true ||
                m.MembershipNumber.Contains(term, StringComparison.OrdinalIgnoreCase)));
    }
}
