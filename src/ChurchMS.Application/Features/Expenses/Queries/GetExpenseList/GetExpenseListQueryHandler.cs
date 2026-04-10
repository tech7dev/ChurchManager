using ChurchMS.Application.Features.Expenses.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.Expenses.Queries.GetExpenseList;

public class GetExpenseListQueryHandler(
    IRepository<Expense> expenseRepository,
    IRepository<ExpenseCategory> categoryRepository)
    : IRequestHandler<GetExpenseListQuery, ApiResponse<PagedResult<ExpenseListDto>>>
{
    public async Task<ApiResponse<PagedResult<ExpenseListDto>>> Handle(
        GetExpenseListQuery request,
        CancellationToken cancellationToken)
    {
        var all = await expenseRepository.FindAsync(e =>
            (!request.Status.HasValue || e.Status == request.Status.Value) &&
            (!request.CategoryId.HasValue || e.CategoryId == request.CategoryId.Value) &&
            (!request.FromDate.HasValue || e.ExpenseDate >= request.FromDate.Value) &&
            (!request.ToDate.HasValue || e.ExpenseDate <= request.ToDate.Value),
            cancellationToken);

        var categories = await categoryRepository.FindAsync(_ => true, cancellationToken);
        var catDict = categories.ToDictionary(c => c.Id, c => c.Name);

        var totalCount = all.Count;
        var paged = all
            .OrderByDescending(e => e.ExpenseDate)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var dtos = paged.Select(e =>
        {
            var dto = e.Adapt<ExpenseListDto>();
            dto.CategoryName = catDict.TryGetValue(e.CategoryId, out var name) ? name : string.Empty;
            return dto;
        }).ToList();

        var result = new PagedResult<ExpenseListDto> { Items = dtos, TotalCount = totalCount, Page = request.Page, PageSize = request.PageSize };
        return ApiResponse<PagedResult<ExpenseListDto>>.SuccessResult(result);
    }
}
