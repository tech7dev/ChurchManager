using ChurchMS.Application.Features.Secretariat.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Secretariat.Commands.RecordBaptism;

public record RecordBaptismCommand(
    Guid MemberId,
    DateOnly BaptismDate,
    Guid? OfficiantMemberId,
    string? Location,
    string? Notes,
    bool IssueCertificate
) : IRequest<ApiResponse<BaptismRecordDto>>;
