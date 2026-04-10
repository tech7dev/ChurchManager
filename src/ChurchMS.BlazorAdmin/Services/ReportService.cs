using ChurchMS.Application.Features.Reports.DTOs;

namespace ChurchMS.BlazorAdmin.Services;

public class ReportService(IHttpClientFactory httpClientFactory, IAuthService authService)
    : ApiClientBase(httpClientFactory, authService)
{
    public async Task<DashboardSummaryDto?> GetDashboardAsync()
    {
        var client = await GetClientAsync();
        return await ReadAsync<DashboardSummaryDto>(
            await client.GetAsync("api/v1/reports/dashboard"));
    }

    public async Task<FinancialSummaryDto?> GetFinancialSummaryAsync(
        DateTime from, DateTime to)
    {
        var client = await GetClientAsync();
        return await ReadAsync<FinancialSummaryDto>(await client.GetAsync(
            $"api/v1/reports/financial?from={from:O}&to={to:O}"));
    }

    public async Task<MemberReportDto?> GetMemberReportAsync(int trendMonths = 12)
    {
        var client = await GetClientAsync();
        return await ReadAsync<MemberReportDto>(await client.GetAsync(
            $"api/v1/reports/members?trendMonths={trendMonths}"));
    }

    public async Task<AttendanceReportDto?> GetAttendanceReportAsync(
        DateTime from, DateTime to)
    {
        var client = await GetClientAsync();
        return await ReadAsync<AttendanceReportDto>(await client.GetAsync(
            $"api/v1/reports/attendance?from={from:O}&to={to:O}"));
    }

    public async Task<ContributionReportDto?> GetContributionReportAsync(
        DateTime from, DateTime to, int page = 1, int pageSize = 50)
    {
        var client = await GetClientAsync();
        return await ReadAsync<ContributionReportDto>(await client.GetAsync(
            $"api/v1/reports/contributions?from={from:O}&to={to:O}&page={page}&pageSize={pageSize}"));
    }

    public async Task<ExpenseReportDto?> GetExpenseReportAsync(
        DateTime from, DateTime to, int page = 1, int pageSize = 50)
    {
        var client = await GetClientAsync();
        return await ReadAsync<ExpenseReportDto>(await client.GetAsync(
            $"api/v1/reports/expenses?from={from:O}&to={to:O}&page={page}&pageSize={pageSize}"));
    }
}
