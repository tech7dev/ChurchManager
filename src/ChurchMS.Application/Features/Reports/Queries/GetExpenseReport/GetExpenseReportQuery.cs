using ChurchMS.Application.Features.Reports.DTOs;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Reports.Queries.GetExpenseReport;

public record GetExpenseReportQuery(
    DateTime From,
    DateTime To,
    Guid? CategoryId = null,
    ExpenseStatus? Status = null,
    int Page = 1,
    int PageSize = 50
) : IRequest<ApiResponse<ExpenseReportDto>>;
