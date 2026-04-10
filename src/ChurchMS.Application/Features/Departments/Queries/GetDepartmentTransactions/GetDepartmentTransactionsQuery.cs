using ChurchMS.Application.Features.Departments.DTOs;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Departments.Queries.GetDepartmentTransactions;

public record GetDepartmentTransactionsQuery(
    Guid DepartmentId,
    DepartmentTransactionType? Type = null,
    DateOnly? FromDate = null,
    DateOnly? ToDate = null,
    int Page = 1,
    int PageSize = 50
) : IRequest<ApiResponse<PagedResult<DepartmentTransactionDto>>>;
