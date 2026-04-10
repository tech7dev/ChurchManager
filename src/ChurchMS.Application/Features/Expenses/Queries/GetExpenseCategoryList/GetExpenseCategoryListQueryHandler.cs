using ChurchMS.Application.Features.Expenses.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.Expenses.Queries.GetExpenseCategoryList;

public class GetExpenseCategoryListQueryHandler(
    IRepository<ExpenseCategory> categoryRepository)
    : IRequestHandler<GetExpenseCategoryListQuery, ApiResponse<IList<ExpenseCategoryDto>>>
{
    public async Task<ApiResponse<IList<ExpenseCategoryDto>>> Handle(
        GetExpenseCategoryListQuery request,
        CancellationToken cancellationToken)
    {
        var categories = await categoryRepository.FindAsync(_ => true, cancellationToken);
        return ApiResponse<IList<ExpenseCategoryDto>>.SuccessResult(categories.Adapt<IList<ExpenseCategoryDto>>());
    }
}
