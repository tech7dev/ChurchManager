using System.Net.Http.Headers;
using System.Net.Http.Json;
using ChurchMS.Shared.Models;

namespace ChurchMS.MAUI.Services;

/// <summary>
/// Wraps HttpClient with automatic JWT injection and standardized deserialization.
/// </summary>
public class ApiService(IHttpClientFactory httpClientFactory, IAuthService authService)
{
    private async Task<HttpClient> GetClientAsync()
    {
        var client = httpClientFactory.CreateClient("ChurchMSApi");
        var token = await authService.GetTokenAsync();
        if (!string.IsNullOrWhiteSpace(token))
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        return client;
    }

    public async Task<T?> GetAsync<T>(string path)
    {
        try
        {
            var client = await GetClientAsync();
            var response = await client.GetAsync(path);
            if (!response.IsSuccessStatusCode) return default;
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<T>>();
            return result?.Success == true ? result.Data : default;
        }
        catch { return default; }
    }

    public async Task<PagedResult<T>?> GetPagedAsync<T>(string path)
    {
        try
        {
            var client = await GetClientAsync();
            var response = await client.GetAsync(path);
            if (!response.IsSuccessStatusCode) return default;
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<PagedResult<T>>>();
            return result?.Success == true ? result.Data : default;
        }
        catch { return default; }
    }

    public async Task<bool> PostAsync(string path, object body)
    {
        try
        {
            var client = await GetClientAsync();
            var response = await client.PostAsJsonAsync(path, body);
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    public async Task<T?> PostAsync<T>(string path, object body)
    {
        try
        {
            var client = await GetClientAsync();
            var response = await client.PostAsJsonAsync(path, body);
            if (!response.IsSuccessStatusCode) return default;
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<T>>();
            return result?.Success == true ? result.Data : default;
        }
        catch { return default; }
    }

    public async Task<bool> PutAsync(string path, object body)
    {
        try
        {
            var client = await GetClientAsync();
            var response = await client.PutAsJsonAsync(path, body);
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }
}
