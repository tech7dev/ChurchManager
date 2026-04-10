using ChurchMS.Application.Features.Reports.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Enums;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Reports.Queries.GetFinancialSummary;

public class GetFinancialSummaryQueryHandler(
    IRepository<Contribution> contributionRepository,
    IRepository<Fund> fundRepository,
    IRepository<Expense> expenseRepository,
    IRepository<ExpenseCategory> expenseCategoryRepository)
    : IRequestHandler<GetFinancialSummaryQuery, ApiResponse<FinancialSummaryDto>>
{
    public async Task<ApiResponse<FinancialSummaryDto>> Handle(
        GetFinancialSummaryQuery request, CancellationToken cancellationToken)
    {
        var fromDate = DateOnly.FromDateTime(request.From);
        var toDate = DateOnly.FromDateTime(request.To);

        var contributions = await contributionRepository.FindAsync(
            c => c.ContributionDate >= fromDate
              && c.ContributionDate <= toDate
              && c.Status == ContributionStatus.Confirmed,
            cancellationToken);

        var expenses = await expenseRepository.FindAsync(
            e => e.ExpenseDate >= fromDate
              && e.ExpenseDate <= toDate
              && e.Status != ExpenseStatus.Rejected,
            cancellationToken);

        var funds = await fundRepository.GetAllAsync(cancellationToken);
        var categories = await expenseCategoryRepository.GetAllAsync(cancellationToken);

        var fundMap = funds.ToDictionary(f => f.Id, f => f.Name);
        var categoryMap = categories.ToDictionary(c => c.Id, c => c.Name);

        // By fund
        var byFund = contributions
            .GroupBy(c => c.FundId)
            .Select(g => new FundBreakdownDto
            {
                FundName = fundMap.GetValueOrDefault(g.Key, "Unknown Fund"),
                Total = g.Sum(c => c.Amount),
                Count = g.Count()
            })
            .OrderByDescending(f => f.Total)
            .ToList();

        // Monthly trend
        var allMonths = new List<MonthlyTrendDto>();
        var cursor = new DateTime(request.From.Year, request.From.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var end = new DateTime(request.To.Year, request.To.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        while (cursor <= end)
        {
            var yr = cursor.Year;
            var mo = cursor.Month;
            allMonths.Add(new MonthlyTrendDto
            {
                Year = yr,
                Month = mo,
                Contributions = contributions
                    .Where(c => c.ContributionDate.Year == yr && c.ContributionDate.Month == mo)
                    .Sum(c => c.Amount),
                Expenses = expenses
                    .Where(e => e.ExpenseDate.Year == yr && e.ExpenseDate.Month == mo)
                    .Sum(e => e.Amount)
            });
            cursor = cursor.AddMonths(1);
        }

        // By expense category
        var byCategory = expenses
            .GroupBy(e => e.CategoryId)
            .Select(g => new ExpenseCategoryBreakdownDto
            {
                Category = categoryMap.GetValueOrDefault(g.Key, "Uncategorized"),
                Total = g.Sum(e => e.Amount),
                Count = g.Count()
            })
            .OrderByDescending(c => c.Total)
            .ToList();

        var totalContributions = contributions.Sum(c => c.Amount);
        var totalExpenses = expenses.Sum(e => e.Amount);

        return ApiResponse<FinancialSummaryDto>.SuccessResult(new FinancialSummaryDto
        {
            TotalContributions = totalContributions,
            TotalExpenses = totalExpenses,
            NetIncome = totalContributions - totalExpenses,
            From = request.From,
            To = request.To,
            ByFund = byFund,
            MonthlyContributionTrend = allMonths,
            ExpensesByCategory = byCategory
        });
    }
}
