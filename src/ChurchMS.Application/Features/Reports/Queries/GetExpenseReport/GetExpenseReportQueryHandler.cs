using ChurchMS.Application.Features.Reports.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Reports.Queries.GetExpenseReport;

public class GetExpenseReportQueryHandler(
    IRepository<Expense> expenseRepository,
    IRepository<ExpenseCategory> categoryRepository)
    : IRequestHandler<GetExpenseReportQuery, ApiResponse<ExpenseReportDto>>
{
    public async Task<ApiResponse<ExpenseReportDto>> Handle(
        GetExpenseReportQuery request, CancellationToken cancellationToken)
    {
        var fromDate = DateOnly.FromDateTime(request.From);
        var toDate = DateOnly.FromDateTime(request.To);

        var expenses = await expenseRepository.FindAsync(
            e => e.ExpenseDate >= fromDate
              && e.ExpenseDate <= toDate
              && (!request.CategoryId.HasValue || e.CategoryId == request.CategoryId.Value)
              && (!request.Status.HasValue || e.Status == request.Status.Value),
            cancellationToken);

        var categories = await categoryRepository.GetAllAsync(cancellationToken);
        var categoryMap = categories.ToDictionary(c => c.Id, c => c.Name);

        var ordered = expenses.OrderByDescending(e => e.ExpenseDate).ToList();
        var totalCount = ordered.Count;
        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        var items = ordered
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new ExpenseReportItemDto
            {
                Id = e.Id,
                Date = e.ExpenseDate.ToDateTime(TimeOnly.MinValue),
                CategoryName = categoryMap.GetValueOrDefault(e.CategoryId, "Uncategorized"),
                Description = e.Title,
                Amount = e.Amount,
                Currency = e.Currency,
                Status = e.Status.ToString(),
                Vendor = e.VendorName,
                Notes = e.Description
            })
            .ToList();

        return ApiResponse<ExpenseReportDto>.SuccessResult(new ExpenseReportDto
        {
            From = request.From,
            To = request.To,
            TotalAmount = ordered.Sum(e => e.Amount),
            TotalCount = totalCount,
            TotalItems = totalCount,
            TotalPages = totalPages,
            Items = items
        });
    }
}
