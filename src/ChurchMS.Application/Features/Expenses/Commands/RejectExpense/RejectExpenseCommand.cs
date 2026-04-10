using ChurchMS.Application.Features.Expenses.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Expenses.Commands.RejectExpense;

public record RejectExpenseCommand(Guid ExpenseId, string Reason) : IRequest<ApiResponse<ExpenseDto>>;
