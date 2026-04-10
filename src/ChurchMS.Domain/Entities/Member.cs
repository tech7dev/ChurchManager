using ChurchMS.Domain.Common;
using ChurchMS.Domain.Enums;

namespace ChurchMS.Domain.Entities;

/// <summary>
/// Registered church member.
/// </summary>
public class Member : TenantEntity
{
    // Personal
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = string.Empty;
    public DateOnly? DateOfBirth { get; set; }
    public Gender? Gender { get; set; }
    public MaritalStatus? MaritalStatus { get; set; }
    public BloodType? BloodType { get; set; }
    public string? PhotoUrl { get; set; }
    public string? NationalId { get; set; }

    // Contact
    public string? Phone { get; set; }
    public string? AlternatePhone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }

    // Church
    public string MembershipNumber { get; set; } = string.Empty;
    public MemberStatus Status { get; set; } = MemberStatus.Active;
    public DateOnly? JoinDate { get; set; }
    public DateOnly? BaptismDate { get; set; }
    public string? BaptizedBy { get; set; }
    public string? Notes { get; set; }

    // Employment
    public string? Occupation { get; set; }
    public string? Employer { get; set; }

    // QR code (unique identifier encoded in QR on member card)
    public string QrCodeValue { get; set; } = string.Empty;

    // Navigation
    public Guid? FamilyId { get; set; }
    public Family? Family { get; set; }
    public FamilyRole? FamilyRole { get; set; }
    public ICollection<MemberCustomFieldValue> CustomFieldValues { get; set; } = new List<MemberCustomFieldValue>();

    // Linked app user (optional — not all members have app accounts)
    public Guid? AppUserId { get; set; }
}
