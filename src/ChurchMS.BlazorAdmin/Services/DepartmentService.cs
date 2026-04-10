using ChurchMS.Shared.Models;

namespace ChurchMS.BlazorAdmin.Services;

public class DepartmentService(IHttpClientFactory httpClientFactory, IAuthService authService)
    : ApiClientBase(httpClientFactory, authService)
{
    public async Task<PagedResult<DepartmentListDto>?> GetDepartmentsAsync(
        int page = 1, int pageSize = 20)
    {
        var client = await GetClientAsync();
        return await ReadPagedAsync<DepartmentListDto>(
            await client.GetAsync($"api/v1/departments?page={page}&pageSize={pageSize}"));
    }
}

public class DepartmentListDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? HeadMemberName { get; set; }
    public int MemberCount { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
