using ChurchMS.Application.Features.Expenses.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Expenses.Commands.SubmitExpense;

public record SubmitExpenseCommand(Guid ExpenseId) : IRequest<ApiResponse<ExpenseDto>>;
