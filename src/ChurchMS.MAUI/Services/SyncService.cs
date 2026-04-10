using ChurchMS.Application.Features.Members.DTOs;
using ChurchMS.MAUI.Models;

namespace ChurchMS.MAUI.Services;

/// <summary>
/// Syncs offline data with the API when connectivity is available.
/// </summary>
public class SyncService(
    ApiService apiService,
    ILocalDatabaseService localDb,
    IAuthService authService)
{
    public async Task SyncMembersAsync()
    {
        if (!await authService.IsAuthenticatedAsync()) return;
        try
        {
            var result = await apiService.GetPagedAsync<MemberListDto>(
                "api/v1/members?page=1&pageSize=1000");
            if (result?.Items is null) return;

            var locals = result.Items.Select(m => new LocalMember
            {
                Id = m.Id,
                MembershipNumber = m.MembershipNumber,
                FirstName = m.FirstName,
                LastName = m.LastName,
                Phone = m.Phone,
                Email = m.Email,
                PhotoUrl = m.PhotoUrl,
                Status = m.Status.ToString(),
                LastSyncedAt = DateTime.UtcNow
            });
            await localDb.SaveMembersAsync(locals);
        }
        catch { /* offline - silently ignore */ }
    }

    public async Task SyncPendingAttendanceAsync()
    {
        if (!await authService.IsAuthenticatedAsync()) return;
        try
        {
            var pending = await localDb.GetPendingAttendanceAsync();
            foreach (var record in pending)
            {
                var success = await apiService.PostAsync(
                    $"api/v1/events/{record.EventId}/attendance",
                    new
                    {
                        memberId = record.MemberId,
                        visitorName = record.VisitorName,
                        attendanceDate = record.AttendanceDate,
                        status = record.Status,
                        recordedByQr = record.RecordedByQr
                    });
                if (success)
                    await localDb.MarkAttendanceSyncedAsync(record.Id);
            }
        }
        catch { /* retry on next sync */ }
    }

    public async Task FullSyncAsync()
    {
        await SyncPendingAttendanceAsync();
        await SyncMembersAsync();
    }
}
