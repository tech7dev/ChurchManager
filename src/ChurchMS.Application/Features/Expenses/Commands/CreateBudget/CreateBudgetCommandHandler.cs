using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.Expenses.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Enums;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.Expenses.Commands.CreateBudget;

public class CreateBudgetCommandHandler(
    IRepository<Budget> budgetRepository,
    IUnitOfWork unitOfWork,
    ITenantService tenantService)
    : IRequestHandler<CreateBudgetCommand, ApiResponse<BudgetDto>>
{
    public async Task<ApiResponse<BudgetDto>> Handle(
        CreateBudgetCommand request,
        CancellationToken cancellationToken)
    {
        var churchId = tenantService.GetCurrentChurchId()
            ?? throw new ForbiddenException("Church context is required.");

        var budget = new Budget
        {
            ChurchId = churchId,
            Name = request.Name,
            Year = request.Year,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Currency = request.Currency,
            TotalAmount = request.TotalAmount,
            Status = BudgetStatus.Draft,
            Notes = request.Notes
        };

        await budgetRepository.AddAsync(budget, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<BudgetDto>.SuccessResult(budget.Adapt<BudgetDto>(), "Budget created successfully.");
    }
}
