using ChurchMS.Domain.Enums;

namespace ChurchMS.Application.Features.Members.DTOs;

public class UpdateMemberRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = string.Empty;
    public DateOnly? DateOfBirth { get; set; }
    public Gender? Gender { get; set; }
    public MaritalStatus? MaritalStatus { get; set; }
    public BloodType? BloodType { get; set; }
    public string? NationalId { get; set; }
    public string? Phone { get; set; }
    public string? AlternatePhone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }
    public MemberStatus Status { get; set; }
    public DateOnly? JoinDate { get; set; }
    public DateOnly? BaptismDate { get; set; }
    public string? BaptizedBy { get; set; }
    public string? Notes { get; set; }
    public string? Occupation { get; set; }
    public string? Employer { get; set; }
    public Guid? FamilyId { get; set; }
    public FamilyRole? FamilyRole { get; set; }
}
