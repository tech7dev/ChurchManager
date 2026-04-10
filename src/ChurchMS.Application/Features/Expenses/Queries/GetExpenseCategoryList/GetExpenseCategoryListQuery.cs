using ChurchMS.Application.Features.Expenses.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Expenses.Queries.GetExpenseCategoryList;

public record GetExpenseCategoryListQuery : IRequest<ApiResponse<IList<ExpenseCategoryDto>>>;
