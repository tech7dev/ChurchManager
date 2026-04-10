using SQLite;

namespace ChurchMS.MAUI.Models;

[Table("Members")]
public class LocalMember
{
    [PrimaryKey]
    public Guid Id { get; set; }
    public string MembershipNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}".Trim();
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? PhotoUrl { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime LastSyncedAt { get; set; }
}
