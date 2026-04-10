using System.Net.Http.Json;
using ChurchMS.BlazorAdmin.Auth;
using ChurchMS.Shared.Models;
using Microsoft.AspNetCore.Components.Authorization;

namespace ChurchMS.BlazorAdmin.Services;

public class AuthService(
    IHttpClientFactory httpClientFactory,
    AuthenticationStateProvider authStateProvider) : IAuthService
{
    private readonly JwtAuthenticationStateProvider _jwtProvider =
        (JwtAuthenticationStateProvider)authStateProvider;

    public async Task<bool> LoginAsync(string email, string password)
    {
        var client = httpClientFactory.CreateClient("ChurchMSApi");
        var response = await client.PostAsJsonAsync("api/v1/auth/login",
            new { email, password });
        if (!response.IsSuccessStatusCode) return false;

        var result = await response.Content
            .ReadFromJsonAsync<ApiResponse<LoginResponseDto>>();
        if (result?.Success != true || result.Data?.Token is null) return false;

        await _jwtProvider.SetTokenAsync(result.Data.Token);
        return true;
    }

    public async Task LogoutAsync()
    {
        await _jwtProvider.ClearTokenAsync();
    }

    public Task<string?> GetTokenAsync() => _jwtProvider.GetTokenAsync();
}

public record LoginResponseDto(
    string Token,
    string RefreshToken,
    DateTime ExpiresAt,
    string UserName,
    string Email,
    Guid? ChurchId,
    IList<string> Roles);
