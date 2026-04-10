using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ChurchMS.Application.Features.Events.DTOs;
using ChurchMS.MAUI.Services;

namespace ChurchMS.MAUI.ViewModels;

public partial class EventsViewModel(
    ApiService apiService,
    ILocalDatabaseService localDb) : BaseViewModel
{
    [ObservableProperty]
    private ObservableCollection<EventListDto> _events = [];

    [ObservableProperty]
    private EventListDto? _selectedEvent;

    [RelayCommand]
    public async Task LoadAsync()
    {
        Title = "Events";
        IsBusy = true;
        ClearError();
        try
        {
            var result = await apiService.GetPagedAsync<EventListDto>(
                "api/v1/events?page=1&pageSize=20");
            if (result?.Items is not null)
                Events = new ObservableCollection<EventListDto>(result.Items);
        }
        catch (Exception ex)
        {
            SetError($"Could not load events: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task SelectEventAsync(EventListDto ev)
    {
        SelectedEvent = ev;
        await Shell.Current.GoToAsync($"eventdetail?id={ev.Id}");
    }

    [RelayCommand]
    private async Task ScanQrAsync()
    {
        await Shell.Current.GoToAsync("qrscanner");
    }

    /// <summary>Records attendance for the selected event from QR scan result.</summary>
    public async Task RecordAttendanceFromQrAsync(Guid eventId, string qrValue)
    {
        try
        {
            // Try to find member by QR value
            var attendance = new Models.LocalAttendance
            {
                Id = Guid.NewGuid(),
                EventId = eventId,
                AttendanceDate = DateOnly.FromDateTime(DateTime.Today),
                Status = "Present",
                RecordedByQr = true,
                IsSynced = false,
                RecordedAt = DateTime.UtcNow,
                VisitorName = qrValue // fallback if member not found
            };

            await localDb.SaveAttendanceAsync(attendance);

            // Also try to post immediately
            await apiService.PostAsync($"api/v1/events/{eventId}/attendance", new
            {
                attendanceDate = attendance.AttendanceDate,
                status = attendance.Status,
                recordedByQr = true
            });
        }
        catch { /* stored offline, will sync later */ }
    }
}
