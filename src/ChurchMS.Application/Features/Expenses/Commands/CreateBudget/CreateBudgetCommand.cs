using ChurchMS.Application.Features.Expenses.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Expenses.Commands.CreateBudget;

public record CreateBudgetCommand(
    string Name,
    int Year,
    DateOnly StartDate,
    DateOnly EndDate,
    string Currency,
    decimal TotalAmount,
    string? Notes) : IRequest<ApiResponse<BudgetDto>>;
