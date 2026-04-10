namespace ChurchMS.Application.Interfaces;

/// <summary>
/// Provides the current tenant (church) context from the authenticated user.
/// </summary>
public interface ITenantService
{
    Guid? GetCurrentChurchId();
    bool IsSuperAdmin();
}
