using ChurchMS.Application.Features.Reports.Queries.GetAttendanceReport;
using ChurchMS.Application.Features.Reports.Queries.GetContributionReport;
using ChurchMS.Application.Features.Reports.Queries.GetDashboardSummary;
using ChurchMS.Application.Features.Reports.Queries.GetExpenseReport;
using ChurchMS.Application.Features.Reports.Queries.GetFinancialSummary;
using ChurchMS.Application.Features.Reports.Queries.GetMemberReport;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchMS.API.Controllers;

/// <summary>
/// Cross-module reporting: dashboard, financial, members, attendance, contributions, expenses.
/// </summary>
[Authorize(Policy = AuthorizationPolicies.RequireChurchAdmin)]
public class ReportsController : BaseApiController
{
    /// <summary>
    /// Dashboard summary — key KPIs for the church at a glance.
    /// </summary>
    [HttpGet("dashboard")]
    [Authorize] // Any authenticated user can see the dashboard
    public async Task<IActionResult> GetDashboard()
        => Ok(await Mediator.Send(new GetDashboardSummaryQuery()));

    /// <summary>
    /// Financial summary — contributions vs expenses with breakdowns by fund and category.
    /// </summary>
    [HttpGet("financial")]
    [Authorize(Policy = AuthorizationPolicies.RequireTreasurer)]
    public async Task<IActionResult> GetFinancialSummary(
        [FromQuery] DateTime from,
        [FromQuery] DateTime to)
        => Ok(await Mediator.Send(new GetFinancialSummaryQuery(from, to)));

    /// <summary>
    /// Member statistics — status breakdown, demographics, and growth trend.
    /// </summary>
    [HttpGet("members")]
    public async Task<IActionResult> GetMemberReport(
        [FromQuery] int trendMonths = 12)
        => Ok(await Mediator.Send(new GetMemberReportQuery(trendMonths)));

    /// <summary>
    /// Event attendance report with per-event breakdown and monthly trend.
    /// </summary>
    [HttpGet("attendance")]
    public async Task<IActionResult> GetAttendanceReport(
        [FromQuery] DateTime from,
        [FromQuery] DateTime to)
        => Ok(await Mediator.Send(new GetAttendanceReportQuery(from, to)));

    /// <summary>
    /// Detailed contribution listing with filters (date range, fund, campaign, member).
    /// </summary>
    [HttpGet("contributions")]
    [Authorize(Policy = AuthorizationPolicies.RequireTreasurer)]
    public async Task<IActionResult> GetContributionReport(
        [FromQuery] DateTime from,
        [FromQuery] DateTime to,
        [FromQuery] Guid? fundId = null,
        [FromQuery] Guid? campaignId = null,
        [FromQuery] Guid? memberId = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50)
        => Ok(await Mediator.Send(
            new GetContributionReportQuery(from, to, fundId, campaignId, memberId, page, pageSize)));

    /// <summary>
    /// Detailed expense listing with filters (date range, category, status).
    /// </summary>
    [HttpGet("expenses")]
    [Authorize(Policy = AuthorizationPolicies.RequireTreasurer)]
    public async Task<IActionResult> GetExpenseReport(
        [FromQuery] DateTime from,
        [FromQuery] DateTime to,
        [FromQuery] Guid? categoryId = null,
        [FromQuery] ExpenseStatus? status = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50)
        => Ok(await Mediator.Send(
            new GetExpenseReportQuery(from, to, categoryId, status, page, pageSize)));
}
