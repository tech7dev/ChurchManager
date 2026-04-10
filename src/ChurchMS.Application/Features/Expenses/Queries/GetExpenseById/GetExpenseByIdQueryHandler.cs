using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.Expenses.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.Expenses.Queries.GetExpenseById;

public class GetExpenseByIdQueryHandler(
    IRepository<Expense> expenseRepository,
    IRepository<ExpenseCategory> categoryRepository)
    : IRequestHandler<GetExpenseByIdQuery, ApiResponse<ExpenseDto>>
{
    public async Task<ApiResponse<ExpenseDto>> Handle(
        GetExpenseByIdQuery request,
        CancellationToken cancellationToken)
    {
        var expense = await expenseRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Expense), request.Id);

        var dto = expense.Adapt<ExpenseDto>();

        var category = await categoryRepository.GetByIdAsync(expense.CategoryId, cancellationToken);
        if (category is not null)
            dto.CategoryName = category.Name;

        return ApiResponse<ExpenseDto>.SuccessResult(dto);
    }
}
