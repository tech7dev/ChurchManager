using ChurchMS.Application.Features.Departments.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Application.Exceptions;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Departments.Commands.RecordDepartmentTransaction;

public class RecordDepartmentTransactionCommandHandler(
    IRepository<Department> departmentRepository,
    IRepository<DepartmentTransaction> transactionRepository,
    ICurrentUserService currentUserService,
    ITenantService tenantService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<RecordDepartmentTransactionCommand, ApiResponse<DepartmentTransactionDto>>
{
    public async Task<ApiResponse<DepartmentTransactionDto>> Handle(
        RecordDepartmentTransactionCommand request, CancellationToken cancellationToken)
    {
        var churchId = tenantService.GetCurrentChurchId()!.Value;

        _ = await departmentRepository.GetByIdAsync(request.DepartmentId, cancellationToken)
            ?? throw new NotFoundException(nameof(Department), request.DepartmentId);

        var transaction = new DepartmentTransaction
        {
            ChurchId = churchId,
            DepartmentId = request.DepartmentId,
            Type = request.Type,
            Amount = request.Amount,
            Description = request.Description,
            Date = request.Date,
            ReferenceNumber = request.ReferenceNumber,
            RecordedByMemberId = currentUserService.GetUserId()
        };

        await transactionRepository.AddAsync(transaction, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<DepartmentTransactionDto>.SuccessResult(new DepartmentTransactionDto
        {
            Id = transaction.Id,
            DepartmentId = transaction.DepartmentId,
            Type = transaction.Type,
            Amount = transaction.Amount,
            Description = transaction.Description,
            Date = transaction.Date,
            ReferenceNumber = transaction.ReferenceNumber,
            RecordedByMemberId = transaction.RecordedByMemberId,
            CreatedAt = transaction.CreatedAt
        });
    }
}
