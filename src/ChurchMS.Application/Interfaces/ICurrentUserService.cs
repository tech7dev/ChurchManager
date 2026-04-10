namespace ChurchMS.Application.Interfaces;

/// <summary>
/// Provides information about the currently authenticated user.
/// </summary>
public interface ICurrentUserService
{
    Guid? GetUserId();
    string? GetUserEmail();
    Guid? GetChurchId();
    IEnumerable<string> GetRoles();
    bool IsInRole(string role);
    bool IsAuthenticated();
}
