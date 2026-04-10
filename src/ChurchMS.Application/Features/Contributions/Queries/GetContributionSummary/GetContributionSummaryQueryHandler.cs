using ChurchMS.Application.Features.Contributions.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Contributions.Queries.GetContributionSummary;

public class GetContributionSummaryQueryHandler(
    IRepository<Contribution> contributionRepository,
    IRepository<Fund> fundRepository,
    ITenantService tenantService)
    : IRequestHandler<GetContributionSummaryQuery, ApiResponse<ContributionSummaryDto>>
{
    public async Task<ApiResponse<ContributionSummaryDto>> Handle(
        GetContributionSummaryQuery request,
        CancellationToken cancellationToken)
    {
        var contributions = await contributionRepository.FindAsync(c =>
            (!request.Year.HasValue || c.ContributionDate.Year == request.Year.Value) &&
            (!request.Month.HasValue || c.ContributionDate.Month == request.Month.Value) &&
            (!request.FundId.HasValue || c.FundId == request.FundId.Value),
            cancellationToken);

        var funds = await fundRepository.FindAsync(_ => true, cancellationToken);
        var fundDict = funds.ToDictionary(f => f.Id, f => f.Name);

        var byFund = contributions
            .GroupBy(c => c.FundId)
            .Select(g => new FundSummaryDto
            {
                FundId = g.Key,
                FundName = fundDict.TryGetValue(g.Key, out var name) ? name : string.Empty,
                TotalAmount = g.Sum(c => c.Amount),
                Count = g.Count()
            })
            .ToList();

        var byMonth = contributions
            .GroupBy(c => new { c.ContributionDate.Year, c.ContributionDate.Month })
            .Select(g => new MonthlySummaryDto
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                TotalAmount = g.Sum(c => c.Amount),
                Count = g.Count()
            })
            .OrderBy(m => m.Year).ThenBy(m => m.Month)
            .ToList();

        var summary = new ContributionSummaryDto
        {
            TotalAmount = contributions.Sum(c => c.Amount),
            TotalCount = contributions.Count,
            Currency = contributions.FirstOrDefault()?.Currency ?? string.Empty,
            ByFund = byFund,
            ByMonth = byMonth
        };

        return ApiResponse<ContributionSummaryDto>.SuccessResult(summary);
    }
}
