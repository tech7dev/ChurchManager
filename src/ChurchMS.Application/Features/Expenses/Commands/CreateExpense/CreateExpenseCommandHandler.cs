using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.Expenses.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Enums;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.Expenses.Commands.CreateExpense;

public class CreateExpenseCommandHandler(
    IRepository<Expense> expenseRepository,
    IRepository<ExpenseCategory> categoryRepository,
    IUnitOfWork unitOfWork,
    ITenantService tenantService)
    : IRequestHandler<CreateExpenseCommand, ApiResponse<ExpenseDto>>
{
    public async Task<ApiResponse<ExpenseDto>> Handle(
        CreateExpenseCommand request,
        CancellationToken cancellationToken)
    {
        var churchId = tenantService.GetCurrentChurchId()
            ?? throw new ForbiddenException("Church context is required.");

        var category = await categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken)
            ?? throw new NotFoundException(nameof(ExpenseCategory), request.CategoryId);

        var expense = new Expense
        {
            ChurchId = churchId,
            Title = request.Title,
            Description = request.Description,
            Amount = request.Amount,
            Currency = request.Currency,
            ExpenseDate = request.ExpenseDate,
            Status = ExpenseStatus.Draft,
            PaymentMethod = request.PaymentMethod,
            VendorName = request.VendorName,
            CategoryId = request.CategoryId,
            BudgetLineId = request.BudgetLineId
        };

        await expenseRepository.AddAsync(expense, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = expense.Adapt<ExpenseDto>();
        dto.CategoryName = category.Name;

        return ApiResponse<ExpenseDto>.SuccessResult(dto, "Expense created successfully.");
    }
}
