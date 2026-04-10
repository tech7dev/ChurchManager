using ChurchMS.Application.Features.Expenses.DTOs;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Expenses.Queries.GetExpenseList;

public record GetExpenseListQuery(
    ExpenseStatus? Status = null,
    Guid? CategoryId = null,
    DateOnly? FromDate = null,
    DateOnly? ToDate = null,
    int Page = 1,
    int PageSize = 10) : IRequest<ApiResponse<PagedResult<ExpenseListDto>>>;
