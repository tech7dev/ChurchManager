using ChurchMS.Application.Features.Expenses.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Expenses.Commands.ApproveExpense;

public record ApproveExpenseCommand(Guid ExpenseId) : IRequest<ApiResponse<ExpenseDto>>;
