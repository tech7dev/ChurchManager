using ChurchMS.Domain.Common;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// A church member assigned to an evangelism team.
/// </summary>
public class EvangelismTeamMember : TenantEntity
{
    public Guid TeamId { get; set; }
    public Guid MemberId { get; set; }
    public DateOnly JoinedDate { get; set; }

    // Navigation
    public EvangelismTeam Team { get; set; } = null!;
    public Member Member { get; set; } = null!;
}
