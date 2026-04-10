namespace ChurchMS.Application.Features.Reports.DTOs;

// ─── Dashboard ────────────────────────────────────────────────────────────────

public class DashboardSummaryDto
{
    // Members
    public int TotalMembers { get; set; }
    public int ActiveMembers { get; set; }
    public int NewMembersThisMonth { get; set; }
    public int TotalFamilies { get; set; }
    public int TotalVisitors { get; set; }

    // Events
    public int UpcomingEventsCount { get; set; }
    public int EventsThisMonth { get; set; }

    // Finance
    public decimal ContributionsThisMonth { get; set; }
    public decimal ContributionsThisYear { get; set; }
    public decimal ExpensesThisMonth { get; set; }
    public string PrimaryCurrency { get; set; } = "USD";

    // Departments
    public int TotalDepartments { get; set; }

    // IT / Subscription
    public int OpenSupportTickets { get; set; }
    public int SmsCreditBalance { get; set; }
    public bool HasActiveSubscription { get; set; }
    public string? ActiveSubscriptionPlan { get; set; }
}

// ─── Financial ────────────────────────────────────────────────────────────────

public class FinancialSummaryDto
{
    public decimal TotalContributions { get; set; }
    public decimal TotalExpenses { get; set; }
    public decimal NetIncome { get; set; }
    public string Currency { get; set; } = "USD";
    public DateTime From { get; set; }
    public DateTime To { get; set; }

    public List<FundBreakdownDto> ByFund { get; set; } = [];
    public List<MonthlyTrendDto> MonthlyContributionTrend { get; set; } = [];
    public List<ExpenseCategoryBreakdownDto> ExpensesByCategory { get; set; } = [];
}

public class FundBreakdownDto
{
    public string FundName { get; set; } = null!;
    public decimal Total { get; set; }
    public int Count { get; set; }
}

public class MonthlyTrendDto
{
    public int Year { get; set; }
    public int Month { get; set; }
    public string MonthLabel => $"{Year}-{Month:D2}";
    public decimal Contributions { get; set; }
    public decimal Expenses { get; set; }
}

public class ExpenseCategoryBreakdownDto
{
    public string Category { get; set; } = null!;
    public decimal Total { get; set; }
    public int Count { get; set; }
}

// ─── Members ──────────────────────────────────────────────────────────────────

public class MemberReportDto
{
    public int Total { get; set; }
    public int Active { get; set; }
    public int Inactive { get; set; }
    public int Deceased { get; set; }
    public int Transferred { get; set; }

    public List<GenderBreakdownDto> ByGender { get; set; } = [];
    public List<AgeGroupDto> ByAgeGroup { get; set; } = [];
    public List<MonthlyGrowthDto> MonthlyGrowth { get; set; } = [];
    public List<MaritalStatusBreakdownDto> ByMaritalStatus { get; set; } = [];
}

public class GenderBreakdownDto
{
    public string Gender { get; set; } = null!;
    public int Count { get; set; }
}

public class AgeGroupDto
{
    public string Label { get; set; } = null!;
    public int Count { get; set; }
}

public class MonthlyGrowthDto
{
    public int Year { get; set; }
    public int Month { get; set; }
    public string MonthLabel => $"{Year}-{Month:D2}";
    public int NewMembers { get; set; }
    public int CumulativeTotal { get; set; }
}

public class MaritalStatusBreakdownDto
{
    public string Status { get; set; } = null!;
    public int Count { get; set; }
}

// ─── Attendance ───────────────────────────────────────────────────────────────

public class AttendanceReportDto
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public int TotalEventSessions { get; set; }
    public int TotalAttendanceRecords { get; set; }
    public double AverageAttendancePerEvent { get; set; }

    public List<EventAttendanceSummaryDto> ByEvent { get; set; } = [];
    public List<MonthlyAttendanceTrendDto> MonthlyTrend { get; set; } = [];
}

public class EventAttendanceSummaryDto
{
    public string EventName { get; set; } = null!;
    public DateTime EventDate { get; set; }
    public int Registered { get; set; }
    public int Attended { get; set; }
    public double AttendanceRate { get; set; }
}

public class MonthlyAttendanceTrendDto
{
    public int Year { get; set; }
    public int Month { get; set; }
    public string MonthLabel => $"{Year}-{Month:D2}";
    public int TotalAttendees { get; set; }
}

// ─── Contribution Detail ──────────────────────────────────────────────────────

public class ContributionReportDto
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public decimal TotalAmount { get; set; }
    public int TotalCount { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
    public List<ContributionReportItemDto> Items { get; set; } = [];
}

public class ContributionReportItemDto
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public string? MemberName { get; set; }
    public string FundName { get; set; } = null!;
    public string? CampaignName { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string? Notes { get; set; }
}

// ─── Expense Detail ───────────────────────────────────────────────────────────

public class ExpenseReportDto
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public decimal TotalAmount { get; set; }
    public int TotalCount { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
    public List<ExpenseReportItemDto> Items { get; set; } = [];
}

public class ExpenseReportItemDto
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public string CategoryName { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string? Vendor { get; set; }
    public string? Notes { get; set; }
}
