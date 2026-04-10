using ChurchMS.Application.Features.Reports.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Reports.Queries.GetFinancialSummary;

public record GetFinancialSummaryQuery(
    DateTime From,
    DateTime To
) : IRequest<ApiResponse<FinancialSummaryDto>>;
