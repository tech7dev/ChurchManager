using ChurchMS.Application.Features.Reports.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Reports.Queries.GetMemberReport;

public record GetMemberReportQuery(
    int TrendMonths = 12
) : IRequest<ApiResponse<MemberReportDto>>;
