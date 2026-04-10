using ChurchMS.Application.Features.GrowthSchool.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.GrowthSchool.Commands.EnrollMember;

public record EnrollMemberCommand(
    Guid CourseId,
    Guid MemberId,
    DateOnly EnrolledDate,
    string? Notes) : IRequest<ApiResponse<GrowthEnrollmentDto>>;
