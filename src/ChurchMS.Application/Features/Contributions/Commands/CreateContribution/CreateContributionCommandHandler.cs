using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.Contributions.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Enums;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.Contributions.Commands.CreateContribution;

public class CreateContributionCommandHandler(
    IRepository<Contribution> contributionRepository,
    IRepository<Fund> fundRepository,
    IRepository<ContributionCampaign> campaignRepository,
    IUnitOfWork unitOfWork,
    ITenantService tenantService)
    : IRequestHandler<CreateContributionCommand, ApiResponse<ContributionDto>>
{
    public async Task<ApiResponse<ContributionDto>> Handle(
        CreateContributionCommand request,
        CancellationToken cancellationToken)
    {
        var churchId = tenantService.GetCurrentChurchId()
            ?? throw new ForbiddenException("Church context is required.");

        var fund = await fundRepository.GetByIdAsync(request.FundId, cancellationToken)
            ?? throw new NotFoundException(nameof(Fund), request.FundId);

        if (request.CampaignId.HasValue)
        {
            var campaign = await campaignRepository.GetByIdAsync(request.CampaignId.Value, cancellationToken)
                ?? throw new NotFoundException(nameof(ContributionCampaign), request.CampaignId.Value);
        }

        var contribution = new Contribution
        {
            ChurchId = churchId,
            ReferenceNumber = GenerateReferenceNumber(),
            Amount = request.Amount,
            Currency = request.Currency,
            ContributionDate = request.ContributionDate,
            Type = request.Type,
            Status = ContributionStatus.Confirmed,
            Notes = request.Notes,
            CheckNumber = request.CheckNumber,
            TransactionReference = request.TransactionReference,
            MemberId = request.MemberId,
            AnonymousDonorName = request.AnonymousDonorName,
            FundId = request.FundId,
            CampaignId = request.CampaignId,
            IsRecurring = request.IsRecurring,
            RecurrenceFrequency = request.RecurrenceFrequency,
            RecurrenceEndDate = request.RecurrenceEndDate
        };

        await contributionRepository.AddAsync(contribution, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = contribution.Adapt<ContributionDto>();
        dto.FundName = fund.Name;

        return ApiResponse<ContributionDto>.SuccessResult(dto, "Contribution recorded successfully.");
    }

    private static string GenerateReferenceNumber()
    {
        var date = DateOnly.FromDateTime(DateTime.UtcNow);
        return $"CTB-{date:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}";
    }
}
