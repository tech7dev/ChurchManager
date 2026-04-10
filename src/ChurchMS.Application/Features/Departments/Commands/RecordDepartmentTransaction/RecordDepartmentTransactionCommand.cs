using ChurchMS.Application.Features.Departments.DTOs;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Departments.Commands.RecordDepartmentTransaction;

public record RecordDepartmentTransactionCommand(
    Guid DepartmentId,
    DepartmentTransactionType Type,
    decimal Amount,
    string Description,
    DateOnly Date,
    string? ReferenceNumber
) : IRequest<ApiResponse<DepartmentTransactionDto>>;
