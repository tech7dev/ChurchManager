using Microsoft.AspNetCore.Identity;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// Application identity role extending ASP.NET Identity role.
/// </summary>
public class AppIdentityRole : IdentityRole<Guid>
{
    public string? Description { get; set; }

    public AppIdentityRole() { }

    public AppIdentityRole(string roleName) : base(roleName) { }

    public AppIdentityRole(string roleName, string description) : base(roleName)
    {
        Description = description;
    }
}
