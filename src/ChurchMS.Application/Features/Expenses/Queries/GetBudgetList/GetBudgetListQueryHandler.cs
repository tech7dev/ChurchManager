using ChurchMS.Application.Features.Expenses.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.Expenses.Queries.GetBudgetList;

public class GetBudgetListQueryHandler(
    IRepository<Budget> budgetRepository,
    IRepository<BudgetLine> budgetLineRepository,
    IRepository<ExpenseCategory> categoryRepository)
    : IRequestHandler<GetBudgetListQuery, ApiResponse<IList<BudgetDto>>>
{
    public async Task<ApiResponse<IList<BudgetDto>>> Handle(
        GetBudgetListQuery request,
        CancellationToken cancellationToken)
    {
        var budgets = await budgetRepository.FindAsync(b =>
            (!request.Year.HasValue || b.Year == request.Year.Value) &&
            (!request.Status.HasValue || b.Status == request.Status.Value),
            cancellationToken);

        var categories = await categoryRepository.FindAsync(_ => true, cancellationToken);
        var catDict = categories.ToDictionary(c => c.Id, c => c.Name);

        var dtos = new List<BudgetDto>();
        foreach (var budget in budgets)
        {
            var lines = await budgetLineRepository.FindAsync(
                l => l.BudgetId == budget.Id, cancellationToken);

            var dto = budget.Adapt<BudgetDto>();
            dto.TotalSpent = lines.Sum(l => l.SpentAmount);
            dto.Lines = lines.Select(l =>
            {
                var lineDto = l.Adapt<BudgetLineDto>();
                lineDto.CategoryName = catDict.TryGetValue(l.CategoryId, out var name) ? name : string.Empty;
                return lineDto;
            }).ToList();
            dtos.Add(dto);
        }

        return ApiResponse<IList<BudgetDto>>.SuccessResult(dtos);
    }
}
