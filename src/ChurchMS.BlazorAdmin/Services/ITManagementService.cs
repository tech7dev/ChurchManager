using ChurchMS.Shared.Models;

namespace ChurchMS.BlazorAdmin.Services;

public class ITManagementService(IHttpClientFactory httpClientFactory, IAuthService authService)
    : ApiClientBase(httpClientFactory, authService)
{
    public async Task<PagedResult<TicketListDto>?> GetTicketsAsync(
        int page = 1, int pageSize = 20)
    {
        var client = await GetClientAsync();
        return await ReadPagedAsync<TicketListDto>(
            await client.GetAsync($"api/v1/it-management/tickets?page={page}&pageSize={pageSize}"));
    }

    public async Task<PagedResult<UserListDto>?> GetUsersAsync(
        int page = 1, int pageSize = 20, string? search = null)
    {
        var client = await GetClientAsync();
        var url = $"api/v1/it-management/users?page={page}&pageSize={pageSize}";
        if (!string.IsNullOrWhiteSpace(search))
            url += $"&searchTerm={Uri.EscapeDataString(search)}";
        return await ReadPagedAsync<UserListDto>(await client.GetAsync(url));
    }
}

public class TicketListDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? SubmittedByName { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class UserListDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public IList<string> Roles { get; set; } = [];
    public DateTime CreatedAt { get; set; }
}
