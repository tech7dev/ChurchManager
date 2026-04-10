using System.Security.Claims;
using ChurchMS.Domain.Entities;

namespace ChurchMS.Application.Interfaces;

/// <summary>
/// JWT and refresh token generation/validation.
/// </summary>
public interface ITokenService
{
    string GenerateAccessToken(AppUser user, IList<string> roles);
    string GenerateRefreshToken();
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}
