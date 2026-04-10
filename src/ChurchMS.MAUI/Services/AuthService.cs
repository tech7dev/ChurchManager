using System.Net.Http.Json;
using System.IdentityModel.Tokens.Jwt;
using ChurchMS.Shared.Models;

namespace ChurchMS.MAUI.Services;

public interface IAuthService
{
    Task<bool> LoginAsync(string email, string password);
    Task LogoutAsync();
    Task<string?> GetTokenAsync();
    Task<bool> IsAuthenticatedAsync();
    string? GetChurchId();
    string? GetUserName();
}

public class AuthService(IHttpClientFactory httpClientFactory) : IAuthService
{
    private const string TokenKey = "churchms_jwt";
    private const string ChurchIdKey = "churchms_church_id";
    private const string UserNameKey = "churchms_username";

    public async Task<bool> LoginAsync(string email, string password)
    {
        try
        {
            var client = httpClientFactory.CreateClient("ChurchMSApi");
            var response = await client.PostAsJsonAsync("api/v1/auth/login",
                new { email, password });
            if (!response.IsSuccessStatusCode) return false;

            var result = await response.Content
                .ReadFromJsonAsync<ApiResponse<LoginResponse>>();
            if (result?.Success != true || result.Data?.Token is null) return false;

            await SecureStorage.SetAsync(TokenKey, result.Data.Token);
            await SecureStorage.SetAsync(ChurchIdKey, result.Data.ChurchId?.ToString() ?? string.Empty);
            await SecureStorage.SetAsync(UserNameKey, result.Data.UserName ?? string.Empty);
            return true;
        }
        catch { return false; }
    }

    public async Task LogoutAsync()
    {
        SecureStorage.Remove(TokenKey);
        SecureStorage.Remove(ChurchIdKey);
        SecureStorage.Remove(UserNameKey);
        await Task.CompletedTask;
    }

    public async Task<string?> GetTokenAsync()
    {
        try { return await SecureStorage.GetAsync(TokenKey); }
        catch { return null; }
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var token = await GetTokenAsync();
        if (string.IsNullOrWhiteSpace(token)) return false;
        try
        {
            var handler = new JwtSecurityTokenHandler();
            if (!handler.CanReadToken(token)) return false;
            var jwt = handler.ReadJwtToken(token);
            return jwt.ValidTo > DateTime.UtcNow;
        }
        catch { return false; }
    }

    public string? GetChurchId() =>
        SecureStorage.GetAsync(ChurchIdKey).GetAwaiter().GetResult();

    public string? GetUserName() =>
        SecureStorage.GetAsync(UserNameKey).GetAwaiter().GetResult();
}

public record LoginResponse(
    string Token,
    string? RefreshToken,
    DateTime ExpiresAt,
    string? UserName,
    string? Email,
    Guid? ChurchId,
    IList<string>? Roles);
