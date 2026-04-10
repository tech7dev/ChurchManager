using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.Expenses.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.Expenses.Commands.CreateExpenseCategory;

public class CreateExpenseCategoryCommandHandler(
    IRepository<ExpenseCategory> categoryRepository,
    IUnitOfWork unitOfWork,
    ITenantService tenantService)
    : IRequestHandler<CreateExpenseCategoryCommand, ApiResponse<ExpenseCategoryDto>>
{
    public async Task<ApiResponse<ExpenseCategoryDto>> Handle(
        CreateExpenseCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var churchId = tenantService.GetCurrentChurchId()
            ?? throw new ForbiddenException("Church context is required.");

        var category = new ExpenseCategory
        {
            ChurchId = churchId,
            Name = request.Name,
            Description = request.Description,
            Color = request.Color
        };

        await categoryRepository.AddAsync(category, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<ExpenseCategoryDto>.SuccessResult(
            category.Adapt<ExpenseCategoryDto>(), "Expense category created.");
    }
}
