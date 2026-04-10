using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Evangelism.Commands.AssignTeamMember;

public class AssignTeamMemberCommandHandler(
    IRepository<EvangelismTeam> teamRepository,
    IRepository<EvangelismTeamMember> teamMemberRepository,
    ITenantService tenantService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<AssignTeamMemberCommand, ApiResponse<bool>>
{
    public async Task<ApiResponse<bool>> Handle(
        AssignTeamMemberCommand request, CancellationToken cancellationToken)
    {
        var churchId = tenantService.GetCurrentChurchId();
        if (!churchId.HasValue)
            return ApiResponse<bool>.FailureResult("Church context required.");

        var team = await teamRepository.GetByIdAsync(request.TeamId, cancellationToken)
            ?? throw new NotFoundException(nameof(EvangelismTeam), request.TeamId);

        var existing = await teamMemberRepository.FindAsync(
            tm => tm.TeamId == request.TeamId && tm.MemberId == request.MemberId,
            cancellationToken);

        if (existing.Count > 0)
            return ApiResponse<bool>.FailureResult("Member is already assigned to this team.");

        var teamMember = new EvangelismTeamMember
        {
            ChurchId = churchId.Value,
            TeamId = team.Id,
            MemberId = request.MemberId,
            JoinedDate = request.JoinedDate
        };

        await teamMemberRepository.AddAsync(teamMember, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<bool>.SuccessResult(true);
    }
}
