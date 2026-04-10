using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.Expenses.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Enums;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.Expenses.Commands.MarkExpensePaid;

public class MarkExpensePaidCommandHandler(
    IRepository<Expense> expenseRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<MarkExpensePaidCommand, ApiResponse<ExpenseDto>>
{
    public async Task<ApiResponse<ExpenseDto>> Handle(
        MarkExpensePaidCommand request,
        CancellationToken cancellationToken)
    {
        var expense = await expenseRepository.GetByIdAsync(request.ExpenseId, cancellationToken)
            ?? throw new NotFoundException(nameof(Expense), request.ExpenseId);

        if (expense.Status != ExpenseStatus.Approved)
            throw new BadRequestException("Only approved expenses can be marked as paid.");

        expense.Status = ExpenseStatus.Paid;
        expense.BankAccountId = request.BankAccountId;
        if (request.ReceiptUrl is not null)
            expense.ReceiptUrl = request.ReceiptUrl;

        expenseRepository.Update(expense);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<ExpenseDto>.SuccessResult(expense.Adapt<ExpenseDto>(), "Expense marked as paid.");
    }
}
