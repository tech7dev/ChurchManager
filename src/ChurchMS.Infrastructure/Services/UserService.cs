using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace ChurchMS.Infrastructure.Services;

/// <summary>
/// Manages application users via ASP.NET Identity's UserManager.
/// </summary>
public class UserService(UserManager<AppUser> userManager) : IUserService
{
    public Task<IReadOnlyList<AppUser>> GetUsersAsync(
        Guid? churchId, CancellationToken cancellationToken = default)
    {
        var query = userManager.Users.Where(u => !u.IsDeleted);

        if (churchId.HasValue)
            query = query.Where(u => u.ChurchId == churchId.Value);

        IReadOnlyList<AppUser> result = query
            .OrderBy(u => u.LastName)
            .ThenBy(u => u.FirstName)
            .ToList();

        return Task.FromResult(result);
    }

    public async Task<AppUser?> GetByIdAsync(Guid id)
        => await userManager.FindByIdAsync(id.ToString());

    public async Task<IList<string>> GetRolesAsync(AppUser user)
        => await userManager.GetRolesAsync(user);

    public async Task<bool> SetActiveStatusAsync(Guid id, bool isActive)
    {
        var user = await userManager.FindByIdAsync(id.ToString());
        if (user is null) return false;

        user.IsActive = isActive;

        if (!isActive)
        {
            await userManager.SetLockoutEnabledAsync(user, true);
            await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
        }
        else
        {
            await userManager.SetLockoutEndDateAsync(user, null);
        }

        var result = await userManager.UpdateAsync(user);
        return result.Succeeded;
    }

    public async Task<bool> AssignRoleAsync(Guid id, string newRole, string? currentRole)
    {
        var user = await userManager.FindByIdAsync(id.ToString());
        if (user is null) return false;

        if (currentRole is not null)
            await userManager.RemoveFromRoleAsync(user, currentRole);

        var result = await userManager.AddToRoleAsync(user, newRole);
        return result.Succeeded;
    }
}
