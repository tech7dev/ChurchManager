using ChurchMS.Domain.Enums;

namespace ChurchMS.Application.Features.Departments.DTOs;

public class DepartmentTransactionDto
{
    public Guid Id { get; set; }
    public Guid DepartmentId { get; set; }
    public DepartmentTransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = null!;
    public DateOnly Date { get; set; }
    public string? ReferenceNumber { get; set; }
    public Guid? RecordedByMemberId { get; set; }
    public string? RecordedByName { get; set; }
    public DateTime CreatedAt { get; set; }
}
