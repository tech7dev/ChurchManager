using ChurchMS.Application.Features.Expenses.DTOs;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Expenses.Commands.CreateExpense;

public record CreateExpenseCommand(
    string Title,
    string? Description,
    decimal Amount,
    string Currency,
    DateOnly ExpenseDate,
    ContributionType PaymentMethod,
    string? VendorName,
    Guid CategoryId,
    Guid? BudgetLineId) : IRequest<ApiResponse<ExpenseDto>>;
