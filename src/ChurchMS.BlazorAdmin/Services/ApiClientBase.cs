using System.Net.Http.Headers;
using System.Net.Http.Json;
using ChurchMS.Shared.Models;

namespace ChurchMS.BlazorAdmin.Services;

/// <summary>
/// Base service that provides an authorized HttpClient for all API module services.
/// </summary>
public abstract class ApiClientBase(IHttpClientFactory httpClientFactory, IAuthService authService)
{
    protected async Task<HttpClient> GetClientAsync()
    {
        var client = httpClientFactory.CreateClient("ChurchMSApi");
        var token = await authService.GetTokenAsync();
        if (!string.IsNullOrWhiteSpace(token))
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        return client;
    }

    protected static async Task<T?> ReadAsync<T>(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode) return default;
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<T>>();
        return result?.Success == true ? result.Data : default;
    }

    protected static async Task<PagedResult<T>?> ReadPagedAsync<T>(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode) return default;
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<PagedResult<T>>>();
        return result?.Success == true ? result.Data : default;
    }
}
