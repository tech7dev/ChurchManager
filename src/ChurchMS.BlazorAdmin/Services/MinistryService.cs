using ChurchMS.Shared.Models;

namespace ChurchMS.BlazorAdmin.Services;

public class MinistryService(IHttpClientFactory httpClientFactory, IAuthService authService)
    : ApiClientBase(httpClientFactory, authService)
{
    // Evangelism
    public async Task<PagedResult<EvangelismCampaignDto>?> GetEvangelismCampaignsAsync(int page = 1, int pageSize = 20)
    {
        var client = await GetClientAsync();
        return await ReadPagedAsync<EvangelismCampaignDto>(await client.GetAsync($"api/v1/evangelism/campaigns?page={page}&pageSize={pageSize}"));
    }

    // Secretariat - Documents
    public async Task<PagedResult<DocumentListDto>?> GetDocumentsAsync(int page = 1, int pageSize = 20)
    {
        var client = await GetClientAsync();
        return await ReadPagedAsync<DocumentListDto>(await client.GetAsync($"api/v1/secretariat/documents?page={page}&pageSize={pageSize}"));
    }

    // Secretariat - Certificates
    public async Task<PagedResult<CertificateListDto>?> GetCertificatesAsync(int page = 1, int pageSize = 20)
    {
        var client = await GetClientAsync();
        return await ReadPagedAsync<CertificateListDto>(await client.GetAsync($"api/v1/secretariat/certificates?page={page}&pageSize={pageSize}"));
    }

    // Multimedia
    public async Task<PagedResult<MediaContentListDto>?> GetMediaContentsAsync(int page = 1, int pageSize = 20)
    {
        var client = await GetClientAsync();
        return await ReadPagedAsync<MediaContentListDto>(await client.GetAsync($"api/v1/multimedia?page={page}&pageSize={pageSize}"));
    }

    // Logistics - Inventory
    public async Task<PagedResult<InventoryItemDto>?> GetInventoryItemsAsync(int page = 1, int pageSize = 20)
    {
        var client = await GetClientAsync();
        return await ReadPagedAsync<InventoryItemDto>(await client.GetAsync($"api/v1/logistics/inventory?page={page}&pageSize={pageSize}"));
    }

    // Logistics - Vehicles
    public async Task<PagedResult<VehicleListDto>?> GetVehiclesAsync(int page = 1, int pageSize = 20)
    {
        var client = await GetClientAsync();
        return await ReadPagedAsync<VehicleListDto>(await client.GetAsync($"api/v1/logistics/vehicles?page={page}&pageSize={pageSize}"));
    }
}

public class EvangelismCampaignDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public int? GoalContacts { get; set; }
    public string? LeaderName { get; set; }
}

public class DocumentListDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CertificateListDto
{
    public Guid Id { get; set; }
    public string CertificateNumber { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string? MemberName { get; set; }
    public DateOnly IssuedDate { get; set; }
}

public class MediaContentListDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string AccessType { get; set; } = string.Empty;
    public decimal? Price { get; set; }
    public int ViewCount { get; set; }
    public int DownloadCount { get; set; }
    public DateTime? PublishedAt { get; set; }
}

public class InventoryItemDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Category { get; set; }
    public int Quantity { get; set; }
    public string? Unit { get; set; }
    public string? Location { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class VehicleListDto
{
    public Guid Id { get; set; }
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int? Year { get; set; }
    public string? PlateNumber { get; set; }
    public int? Capacity { get; set; }
    public string Status { get; set; } = string.Empty;
}
