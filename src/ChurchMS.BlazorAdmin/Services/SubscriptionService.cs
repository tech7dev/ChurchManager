using ChurchMS.Shared.Models;

namespace ChurchMS.BlazorAdmin.Services;

public class SubscriptionService(IHttpClientFactory httpClientFactory, IAuthService authService)
    : ApiClientBase(httpClientFactory, authService)
{
    public async Task<SubscriptionDto?> GetCurrentSubscriptionAsync()
    {
        var client = await GetClientAsync();
        return await ReadAsync<SubscriptionDto>(
            await client.GetAsync("api/v1/subscriptions/current"));
    }

    public async Task<PagedResult<InvoiceListDto>?> GetInvoicesAsync(
        int page = 1, int pageSize = 20)
    {
        var client = await GetClientAsync();
        return await ReadPagedAsync<InvoiceListDto>(
            await client.GetAsync($"api/v1/subscriptions/invoices?page={page}&pageSize={pageSize}"));
    }

    public async Task<SmsCreditDto?> GetSmsCreditBalanceAsync()
    {
        var client = await GetClientAsync();
        return await ReadAsync<SmsCreditDto>(
            await client.GetAsync("api/v1/subscriptions/sms-credits"));
    }
}

public class SubscriptionDto
{
    public Guid Id { get; set; }
    public string Plan { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string BillingCycle { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public DateTime? CurrentPeriodEnd { get; set; }
    public bool AutoRenew { get; set; }
}

public class InvoiceListDto
{
    public Guid Id { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateOnly DueDate { get; set; }
    public DateTime? PaidAt { get; set; }
}

public class SmsCreditDto
{
    public int Balance { get; set; }
    public int TotalPurchased { get; set; }
    public int TotalConsumed { get; set; }
}
