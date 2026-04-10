using ChurchMS.Application.Features.Expenses.DTOs;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Expenses.Queries.GetBudgetList;

public record GetBudgetListQuery(
    int? Year = null,
    BudgetStatus? Status = null) : IRequest<ApiResponse<IList<BudgetDto>>>;
