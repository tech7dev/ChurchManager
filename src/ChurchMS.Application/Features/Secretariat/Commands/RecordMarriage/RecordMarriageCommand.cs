using ChurchMS.Application.Features.Secretariat.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Secretariat.Commands.RecordMarriage;

public record RecordMarriageCommand(
    Guid Spouse1MemberId,
    Guid? Spouse2MemberId,
    string? Spouse2Name,
    string? Spouse2Phone,
    DateOnly MarriageDate,
    Guid? OfficiantMemberId,
    string? Location,
    string? Notes,
    bool IssueCertificate
) : IRequest<ApiResponse<MarriageRecordDto>>;
