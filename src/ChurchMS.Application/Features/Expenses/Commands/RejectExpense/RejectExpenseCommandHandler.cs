using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.Expenses.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Enums;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.Expenses.Commands.RejectExpense;

public class RejectExpenseCommandHandler(
    IRepository<Expense> expenseRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<RejectExpenseCommand, ApiResponse<ExpenseDto>>
{
    public async Task<ApiResponse<ExpenseDto>> Handle(
        RejectExpenseCommand request,
        CancellationToken cancellationToken)
    {
        var expense = await expenseRepository.GetByIdAsync(request.ExpenseId, cancellationToken)
            ?? throw new NotFoundException(nameof(Expense), request.ExpenseId);

        if (expense.Status != ExpenseStatus.Submitted)
            throw new BadRequestException("Only submitted expenses can be rejected.");

        expense.Status = ExpenseStatus.Rejected;
        expense.RejectionReason = request.Reason;
        expenseRepository.Update(expense);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<ExpenseDto>.SuccessResult(expense.Adapt<ExpenseDto>(), "Expense rejected.");
    }
}
