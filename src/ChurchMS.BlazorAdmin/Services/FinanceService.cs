using ChurchMS.Application.Features.Contributions.DTOs;
using ChurchMS.Shared.Models;

namespace ChurchMS.BlazorAdmin.Services;

public class FinanceService(IHttpClientFactory httpClientFactory, IAuthService authService)
    : ApiClientBase(httpClientFactory, authService)
{
    // Contributions
    public async Task<PagedResult<ContributionListDto>?> GetContributionsAsync(
        int page = 1, int pageSize = 20, Guid? fundId = null, Guid? memberId = null)
    {
        var client = await GetClientAsync();
        var url = $"api/v1/contributions?page={page}&pageSize={pageSize}";
        if (fundId.HasValue) url += $"&fundId={fundId}";
        if (memberId.HasValue) url += $"&memberId={memberId}";
        return await ReadPagedAsync<ContributionListDto>(await client.GetAsync(url));
    }

    public async Task<ContributionDto?> GetContributionByIdAsync(Guid id)
    {
        var client = await GetClientAsync();
        return await ReadAsync<ContributionDto>(
            await client.GetAsync($"api/v1/contributions/{id}"));
    }

    // Funds
    public async Task<PagedResult<FundDto>?> GetFundsAsync(int page = 1, int pageSize = 50)
    {
        var client = await GetClientAsync();
        return await ReadPagedAsync<FundDto>(
            await client.GetAsync($"api/v1/contributions/funds?page={page}&pageSize={pageSize}"));
    }

    // Campaigns
    public async Task<PagedResult<CampaignDto>?> GetCampaignsAsync(int page = 1, int pageSize = 20)
    {
        var client = await GetClientAsync();
        return await ReadPagedAsync<CampaignDto>(
            await client.GetAsync($"api/v1/contributions/campaigns?page={page}&pageSize={pageSize}"));
    }

    // Pledges
    public async Task<PagedResult<PledgeDto>?> GetPledgesAsync(
        int page = 1, int pageSize = 20, Guid? memberId = null)
    {
        var client = await GetClientAsync();
        var url = $"api/v1/contributions/pledges?page={page}&pageSize={pageSize}";
        if (memberId.HasValue) url += $"&memberId={memberId}";
        return await ReadPagedAsync<PledgeDto>(await client.GetAsync(url));
    }

    // Expenses
    public async Task<PagedResult<ExpenseListItemDto>?> GetExpensesAsync(
        int page = 1, int pageSize = 20)
    {
        var client = await GetClientAsync();
        return await ReadPagedAsync<ExpenseListItemDto>(
            await client.GetAsync($"api/v1/expenses?page={page}&pageSize={pageSize}"));
    }
}

/// <summary>Local list DTO for expense display until Application DTOs are finalized.</summary>
public class ExpenseListItemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? CategoryName { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public DateOnly ExpenseDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? VendorName { get; set; }
}
