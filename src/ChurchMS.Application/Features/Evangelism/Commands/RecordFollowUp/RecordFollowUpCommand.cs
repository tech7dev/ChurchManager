using ChurchMS.Application.Features.Evangelism.DTOs;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Evangelism.Commands.RecordFollowUp;

public record RecordFollowUpCommand(
    Guid ContactId,
    FollowUpMethod Method,
    DateOnly FollowUpDate,
    string? Notes,
    Guid? ConductedByMemberId
) : IRequest<ApiResponse<EvangelismFollowUpDto>>;
