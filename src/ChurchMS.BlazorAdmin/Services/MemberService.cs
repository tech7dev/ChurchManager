using ChurchMS.Application.Features.Members.DTOs;
using ChurchMS.Shared.Models;

namespace ChurchMS.BlazorAdmin.Services;

public class MemberService(IHttpClientFactory httpClientFactory, IAuthService authService)
    : ApiClientBase(httpClientFactory, authService)
{
    public async Task<PagedResult<MemberListDto>?> GetMembersAsync(
        int page = 1, int pageSize = 20, string? search = null)
    {
        var client = await GetClientAsync();
        var url = $"api/v1/members?page={page}&pageSize={pageSize}";
        if (!string.IsNullOrWhiteSpace(search))
            url += $"&searchTerm={Uri.EscapeDataString(search)}";
        return await ReadPagedAsync<MemberListDto>(await client.GetAsync(url));
    }

    public async Task<MemberDto?> GetByIdAsync(Guid id)
    {
        var client = await GetClientAsync();
        return await ReadAsync<MemberDto>(await client.GetAsync($"api/v1/members/{id}"));
    }

    public async Task<bool> CreateAsync(CreateMemberRequest request)
    {
        var client = await GetClientAsync();
        var response = await client.PostAsJsonAsync("api/v1/members", request);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateMemberRequest request)
    {
        var client = await GetClientAsync();
        var response = await client.PutAsJsonAsync($"api/v1/members/{id}", request);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var client = await GetClientAsync();
        var response = await client.DeleteAsync($"api/v1/members/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<PagedResult<VisitorDto>?> GetVisitorsAsync(
        int page = 1, int pageSize = 20, string? search = null)
    {
        var client = await GetClientAsync();
        var url = $"api/v1/visitors?page={page}&pageSize={pageSize}";
        if (!string.IsNullOrWhiteSpace(search))
            url += $"&searchTerm={Uri.EscapeDataString(search)}";
        return await ReadPagedAsync<VisitorDto>(await client.GetAsync(url));
    }

    public async Task<PagedResult<FamilyDto>?> GetFamiliesAsync(
        int page = 1, int pageSize = 20)
    {
        var client = await GetClientAsync();
        return await ReadPagedAsync<FamilyDto>(
            await client.GetAsync($"api/v1/members/families?page={page}&pageSize={pageSize}"));
    }
}
