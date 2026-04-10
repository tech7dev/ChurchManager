namespace ChurchMS.Application.Features.Contributions.DTOs;

public class ContributionSummaryDto
{
    public decimal TotalAmount { get; set; }
    public int TotalCount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public decimal TotalByFund { get; set; }
    public string? FundName { get; set; }
    public IList<FundSummaryDto> ByFund { get; set; } = new List<FundSummaryDto>();
    public IList<MonthlySummaryDto> ByMonth { get; set; } = new List<MonthlySummaryDto>();
}

public class FundSummaryDto
{
    public Guid FundId { get; set; }
    public string FundName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public int Count { get; set; }
}

public class MonthlySummaryDto
{
    public int Year { get; set; }
    public int Month { get; set; }
    public string MonthName => new DateTime(Year, Month, 1).ToString("MMMM yyyy");
    public decimal TotalAmount { get; set; }
    public int Count { get; set; }
}
