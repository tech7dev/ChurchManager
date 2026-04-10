using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace ChurchMS.BlazorAdmin.Auth;

/// <summary>
/// Custom Blazor authentication state provider that reads a JWT from protected local storage.
/// </summary>
public class JwtAuthenticationStateProvider(ProtectedLocalStorage localStorage)
    : AuthenticationStateProvider
{
    private const string TokenKey = "churchms_jwt";
    private static readonly AuthenticationState Anonymous =
        new(new ClaimsPrincipal(new ClaimsIdentity()));

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var result = await localStorage.GetAsync<string>(TokenKey);
            if (!result.Success || string.IsNullOrWhiteSpace(result.Value))
                return Anonymous;

            var principal = ParseToken(result.Value);
            return principal is null ? Anonymous : new AuthenticationState(principal);
        }
        catch
        {
            return Anonymous;
        }
    }

    public async Task SetTokenAsync(string token)
    {
        await localStorage.SetAsync(TokenKey, token);
        var principal = ParseToken(token);
        NotifyAuthenticationStateChanged(
            Task.FromResult(principal is null ? Anonymous : new AuthenticationState(principal)));
    }

    public async Task ClearTokenAsync()
    {
        await localStorage.DeleteAsync(TokenKey);
        NotifyAuthenticationStateChanged(Task.FromResult(Anonymous));
    }

    public async Task<string?> GetTokenAsync()
    {
        try
        {
            var result = await localStorage.GetAsync<string>(TokenKey);
            return result.Success ? result.Value : null;
        }
        catch { return null; }
    }

    private static ClaimsPrincipal? ParseToken(string token)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            if (!handler.CanReadToken(token)) return null;
            var jwt = handler.ReadJwtToken(token);
            if (jwt.ValidTo < DateTime.UtcNow) return null;
            var claims = jwt.Claims.ToList();
            var identity = new ClaimsIdentity(claims, "jwt");
            return new ClaimsPrincipal(identity);
        }
        catch { return null; }
    }
}
