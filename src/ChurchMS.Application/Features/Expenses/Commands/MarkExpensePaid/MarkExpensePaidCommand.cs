using ChurchMS.Application.Features.Expenses.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Expenses.Commands.MarkExpensePaid;

public record MarkExpensePaidCommand(
    Guid ExpenseId,
    Guid? BankAccountId,
    string? ReceiptUrl) : IRequest<ApiResponse<ExpenseDto>>;
