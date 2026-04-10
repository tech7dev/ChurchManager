using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.Expenses.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Enums;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.Expenses.Commands.ApproveExpense;

public class ApproveExpenseCommandHandler(
    IRepository<Expense> expenseRepository,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService)
    : IRequestHandler<ApproveExpenseCommand, ApiResponse<ExpenseDto>>
{
    public async Task<ApiResponse<ExpenseDto>> Handle(
        ApproveExpenseCommand request,
        CancellationToken cancellationToken)
    {
        var expense = await expenseRepository.GetByIdAsync(request.ExpenseId, cancellationToken)
            ?? throw new NotFoundException(nameof(Expense), request.ExpenseId);

        if (expense.Status != ExpenseStatus.Submitted)
            throw new BadRequestException("Only submitted expenses can be approved.");

        expense.Status = ExpenseStatus.Approved;
        expense.ApprovedAt = DateTime.UtcNow;

        expense.ApprovedByMemberId = currentUserService.GetUserId();

        expenseRepository.Update(expense);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<ExpenseDto>.SuccessResult(expense.Adapt<ExpenseDto>(), "Expense approved.");
    }
}
