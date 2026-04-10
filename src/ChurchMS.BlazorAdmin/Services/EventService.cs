using ChurchMS.Application.Features.Events.DTOs;
using ChurchMS.Shared.Models;

namespace ChurchMS.BlazorAdmin.Services;

public class EventService(IHttpClientFactory httpClientFactory, IAuthService authService)
    : ApiClientBase(httpClientFactory, authService)
{
    public async Task<PagedResult<EventListDto>?> GetEventsAsync(
        int page = 1, int pageSize = 20, string? search = null)
    {
        var client = await GetClientAsync();
        var url = $"api/v1/events?page={page}&pageSize={pageSize}";
        if (!string.IsNullOrWhiteSpace(search))
            url += $"&searchTerm={Uri.EscapeDataString(search)}";
        return await ReadPagedAsync<EventListDto>(await client.GetAsync(url));
    }

    public async Task<EventDto?> GetByIdAsync(Guid id)
    {
        var client = await GetClientAsync();
        return await ReadAsync<EventDto>(await client.GetAsync($"api/v1/events/{id}"));
    }

    public async Task<PagedResult<EventRegistrationDto>?> GetRegistrationsAsync(
        Guid eventId, int page = 1, int pageSize = 50)
    {
        var client = await GetClientAsync();
        return await ReadPagedAsync<EventRegistrationDto>(await client.GetAsync(
            $"api/v1/events/{eventId}/registrations?page={page}&pageSize={pageSize}"));
    }

    public async Task<PagedResult<EventAttendanceDto>?> GetAttendanceAsync(
        Guid eventId, int page = 1, int pageSize = 50)
    {
        var client = await GetClientAsync();
        return await ReadPagedAsync<EventAttendanceDto>(await client.GetAsync(
            $"api/v1/events/{eventId}/attendance?page={page}&pageSize={pageSize}"));
    }
}
