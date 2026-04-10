using ChurchMS.Application.Features.Members.DTOs;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Members.Commands.CreateVisitor;

public class CreateVisitorCommand : IRequest<ApiResponse<VisitorDto>>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public Gender? Gender { get; set; }
    public DateOnly? FirstVisitDate { get; set; }
    public string? HowHeardAboutUs { get; set; }
    public string? Notes { get; set; }
    public Guid? AssignedToMemberId { get; set; }
}
