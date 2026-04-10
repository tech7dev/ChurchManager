using ChurchMS.Application.Features.SundaySchool.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.SundaySchool.Commands.EnrollMember;

public record EnrollMemberCommand(
    Guid ClassId,
    Guid MemberId,
    DateOnly EnrolledDate,
    string? Notes) : IRequest<ApiResponse<EnrollmentDto>>;
