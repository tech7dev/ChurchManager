using System.Security.Claims;
using ChurchMS.Application.Interfaces;
using ChurchMS.Shared.Constants;
using Microsoft.AspNetCore.Http;

namespace ChurchMS.Infrastructure.Services;

public class TenantService(IHttpContextAccessor httpContextAccessor) : ITenantService
{
    public Guid? GetCurrentChurchId()
    {
        var churchId = httpContextAccessor.HttpContext?.User?.FindFirstValue("churchId");
        return churchId is not null ? Guid.Parse(churchId) : null;
    }

    public bool IsSuperAdmin()
    {
        return httpContextAccessor.HttpContext?.User?.IsInRole(AppConstants.Roles.SuperAdmin) ?? false;
    }
}
