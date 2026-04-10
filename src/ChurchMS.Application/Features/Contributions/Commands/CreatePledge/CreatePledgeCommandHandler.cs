using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.Contributions.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Enums;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.Contributions.Commands.CreatePledge;

public class CreatePledgeCommandHandler(
    IRepository<Pledge> pledgeRepository,
    IRepository<Member> memberRepository,
    IRepository<Fund> fundRepository,
    IUnitOfWork unitOfWork,
    ITenantService tenantService)
    : IRequestHandler<CreatePledgeCommand, ApiResponse<PledgeDto>>
{
    public async Task<ApiResponse<PledgeDto>> Handle(
        CreatePledgeCommand request,
        CancellationToken cancellationToken)
    {
        var churchId = tenantService.GetCurrentChurchId()
            ?? throw new ForbiddenException("Church context is required.");

        var member = await memberRepository.GetByIdAsync(request.MemberId, cancellationToken)
            ?? throw new NotFoundException(nameof(Member), request.MemberId);

        var fund = await fundRepository.GetByIdAsync(request.FundId, cancellationToken)
            ?? throw new NotFoundException(nameof(Fund), request.FundId);

        var pledge = new Pledge
        {
            ChurchId = churchId,
            MemberId = request.MemberId,
            FundId = request.FundId,
            CampaignId = request.CampaignId,
            PledgedAmount = request.PledgedAmount,
            PaidAmount = 0,
            Currency = request.Currency,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Status = PledgeStatus.Active,
            Notes = request.Notes
        };

        await pledgeRepository.AddAsync(pledge, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = pledge.Adapt<PledgeDto>();
        dto.MemberName = $"{member.FirstName} {member.LastName}";
        dto.FundName = fund.Name;

        return ApiResponse<PledgeDto>.SuccessResult(dto, "Pledge created successfully.");
    }
}
