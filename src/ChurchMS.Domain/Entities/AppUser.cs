using Microsoft.AspNetCore.Identity;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// Application user extending ASP.NET Identity.
/// Does not inherit BaseEntity because IdentityUser manages its own Id.
/// </summary>
public class AppUser : IdentityUser<Guid>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public Guid? ChurchId { get; set; }
    public Church? Church { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
}
