using System.Security.Claims;
using ChurchMS.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace ChurchMS.Infrastructure.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public Guid? GetUserId()
    {
        var userId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        return userId is not null ? Guid.Parse(userId) : null;
    }

    public string? GetUserEmail()
    {
        return httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);
    }

    public Guid? GetChurchId()
    {
        var churchId = httpContextAccessor.HttpContext?.User?.FindFirstValue("churchId");
        return churchId is not null ? Guid.Parse(churchId) : null;
    }

    public IEnumerable<string> GetRoles()
    {
        return httpContextAccessor.HttpContext?.User?.Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => c.Value) ?? [];
    }

    public bool IsInRole(string role)
    {
        return httpContextAccessor.HttpContext?.User?.IsInRole(role) ?? false;
    }

    public bool IsAuthenticated()
    {
        return httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
    }
}
