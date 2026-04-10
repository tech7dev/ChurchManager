using ChurchMS.Application.Features.Expenses.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Expenses.Commands.CreateExpenseCategory;

public record CreateExpenseCategoryCommand(
    string Name,
    string? Description,
    string? Color) : IRequest<ApiResponse<ExpenseCategoryDto>>;
