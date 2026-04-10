using ChurchMS.Application.Features.Expenses.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Expenses.Queries.GetExpenseById;

public record GetExpenseByIdQuery(Guid Id) : IRequest<ApiResponse<ExpenseDto>>;
