using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.Expenses.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Enums;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.Expenses.Commands.SubmitExpense;

public class SubmitExpenseCommandHandler(
    IRepository<Expense> expenseRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<SubmitExpenseCommand, ApiResponse<ExpenseDto>>
{
    public async Task<ApiResponse<ExpenseDto>> Handle(
        SubmitExpenseCommand request,
        CancellationToken cancellationToken)
    {
        var expense = await expenseRepository.GetByIdAsync(request.ExpenseId, cancellationToken)
            ?? throw new NotFoundException(nameof(Expense), request.ExpenseId);

        if (expense.Status != ExpenseStatus.Draft)
            throw new BadRequestException("Only draft expenses can be submitted.");

        expense.Status = ExpenseStatus.Submitted;
        expenseRepository.Update(expense);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<ExpenseDto>.SuccessResult(expense.Adapt<ExpenseDto>(), "Expense submitted for approval.");
    }
}
