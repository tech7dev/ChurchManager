using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.Contributions.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Enums;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.Contributions.Commands.CreateCampaign;

public class CreateCampaignCommandHandler(
    IRepository<ContributionCampaign> campaignRepository,
    IUnitOfWork unitOfWork,
    ITenantService tenantService)
    : IRequestHandler<CreateCampaignCommand, ApiResponse<CampaignDto>>
{
    public async Task<ApiResponse<CampaignDto>> Handle(
        CreateCampaignCommand request,
        CancellationToken cancellationToken)
    {
        var churchId = tenantService.GetCurrentChurchId()
            ?? throw new ForbiddenException("Church context is required.");

        var campaign = new ContributionCampaign
        {
            ChurchId = churchId,
            Name = request.Name,
            Description = request.Description,
            TargetAmount = request.TargetAmount,
            Currency = request.Currency,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            FundId = request.FundId,
            Status = CampaignStatus.Draft
        };

        await campaignRepository.AddAsync(campaign, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<CampaignDto>.SuccessResult(campaign.Adapt<CampaignDto>(), "Campaign created successfully.");
    }
}
