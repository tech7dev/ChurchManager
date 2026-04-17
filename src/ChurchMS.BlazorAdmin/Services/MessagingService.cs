using ChurchMS.Shared.Models;

namespace ChurchMS.BlazorAdmin.Services;

public class MessagingService(IHttpClientFactory httpClientFactory, IAuthService authService)
    : ApiClientBase(httpClientFactory, authService)
{
    public async Task<PagedResult<MessageCampaignListDto>?> GetCampaignsAsync(
        int page = 1, int pageSize = 20)
    {
        var client = await GetClientAsync();
        return await ReadPagedAsync<MessageCampaignListDto>(
            await client.GetAsync($"api/v1/messaging/campaigns?page={page}&pageSize={pageSize}"));
    }

    public async Task<PagedResult<AppointmentListDto>?> GetAppointmentsAsync(
        int page = 1, int pageSize = 20)
    {
        var client = await GetClientAsync();
        return await ReadPagedAsync<AppointmentListDto>(
            await client.GetAsync($"api/v1/messaging/appointments?page={page}&pageSize={pageSize}"));
    }

    public async Task<PagedResult<NotificationListDto>?> GetNotificationsAsync(int page = 1, int pageSize = 20)
    {
        var client = await GetClientAsync();
        return await ReadPagedAsync<NotificationListDto>(await client.GetAsync($"api/v1/notifications?page={page}&pageSize={pageSize}"));
    }
}

public class MessageCampaignListDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Channel { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int RecipientCount { get; set; }
    public DateTime? SentAt { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class AppointmentListDto
{
    public Guid Id { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string MemberName { get; set; } = string.Empty;
    public string? AssigneeName { get; set; }
    public DateTime? ScheduledAt { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class NotificationListDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
}
