using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Evangelism.Commands.AssignTeamMember;

public record AssignTeamMemberCommand(
    Guid TeamId,
    Guid MemberId,
    DateOnly JoinedDate
) : IRequest<ApiResponse<bool>>;
