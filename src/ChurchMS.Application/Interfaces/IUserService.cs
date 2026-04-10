using ChurchMS.Domain.Entities;

namespace ChurchMS.Application.Interfaces;

/// <summary>
/// Service for managing application users via ASP.NET Identity.
/// </summary>
public interface IUserService
{
    Task<IReadOnlyList<AppUser>> GetUsersAsync(Guid? churchId, CancellationToken cancellationToken = default);
    Task<AppUser?> GetByIdAsync(Guid id);
    Task<IList<string>> GetRolesAsync(AppUser user);
    Task<bool> SetActiveStatusAsync(Guid id, bool isActive);
    Task<bool> AssignRoleAsync(Guid id, string newRole, string? currentRole);
}
