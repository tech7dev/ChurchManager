using ChurchMS.Application.Features.Reports.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Enums;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Reports.Queries.GetContributionReport;

public class GetContributionReportQueryHandler(
    IRepository<Contribution> contributionRepository,
    IRepository<Fund> fundRepository,
    IRepository<ContributionCampaign> campaignRepository,
    IRepository<Member> memberRepository)
    : IRequestHandler<GetContributionReportQuery, ApiResponse<ContributionReportDto>>
{
    public async Task<ApiResponse<ContributionReportDto>> Handle(
        GetContributionReportQuery request, CancellationToken cancellationToken)
    {
        var fromDate = DateOnly.FromDateTime(request.From);
        var toDate = DateOnly.FromDateTime(request.To);

        var contributions = await contributionRepository.FindAsync(
            c => c.ContributionDate >= fromDate
              && c.ContributionDate <= toDate
              && (!request.FundId.HasValue || c.FundId == request.FundId.Value)
              && (!request.CampaignId.HasValue || c.CampaignId == request.CampaignId.Value)
              && (!request.MemberId.HasValue || c.MemberId == request.MemberId.Value),
            cancellationToken);

        var funds = await fundRepository.GetAllAsync(cancellationToken);
        var campaigns = await campaignRepository.GetAllAsync(cancellationToken);
        var members = await memberRepository.GetAllAsync(cancellationToken);

        var fundMap = funds.ToDictionary(f => f.Id, f => f.Name);
        var campaignMap = campaigns.ToDictionary(c => c.Id, c => c.Name);
        var memberMap = members.ToDictionary(
            m => m.Id,
            m => $"{m.FirstName} {m.LastName}".Trim());

        var ordered = contributions.OrderByDescending(c => c.ContributionDate).ToList();
        var totalCount = ordered.Count;
        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        var items = ordered
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(c => new ContributionReportItemDto
            {
                Id = c.Id,
                Date = c.ContributionDate.ToDateTime(TimeOnly.MinValue),
                MemberName = c.MemberId.HasValue
                    ? memberMap.GetValueOrDefault(c.MemberId.Value)
                    : c.AnonymousDonorName,
                FundName = fundMap.GetValueOrDefault(c.FundId, "Unknown Fund"),
                CampaignName = c.CampaignId.HasValue
                    ? campaignMap.GetValueOrDefault(c.CampaignId.Value)
                    : null,
                Amount = c.Amount,
                Currency = c.Currency,
                Type = c.Type.ToString(),
                Status = c.Status.ToString(),
                Notes = c.Notes
            })
            .ToList();

        return ApiResponse<ContributionReportDto>.SuccessResult(new ContributionReportDto
        {
            From = request.From,
            To = request.To,
            TotalAmount = ordered.Where(c => c.Status == ContributionStatus.Confirmed).Sum(c => c.Amount),
            TotalCount = totalCount,
            TotalItems = totalCount,
            TotalPages = totalPages,
            Items = items
        });
    }
}
