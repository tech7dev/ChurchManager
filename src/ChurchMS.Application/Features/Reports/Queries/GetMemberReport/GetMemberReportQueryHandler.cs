using ChurchMS.Application.Features.Reports.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Enums;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Reports.Queries.GetMemberReport;

public class GetMemberReportQueryHandler(
    IRepository<Member> memberRepository,
    IDateTimeService dateTimeService)
    : IRequestHandler<GetMemberReportQuery, ApiResponse<MemberReportDto>>
{
    public async Task<ApiResponse<MemberReportDto>> Handle(
        GetMemberReportQuery request, CancellationToken cancellationToken)
    {
        var members = await memberRepository.GetAllAsync(cancellationToken);
        var now = dateTimeService.UtcNow;
        var today = DateOnly.FromDateTime(now);

        // Gender breakdown (nullable Gender)
        var byGender = members
            .GroupBy(m => m.Gender?.ToString() ?? "Unknown")
            .Select(g => new GenderBreakdownDto { Gender = g.Key, Count = g.Count() })
            .OrderByDescending(g => g.Count)
            .ToList();

        // Marital status breakdown (nullable)
        var byMaritalStatus = members
            .GroupBy(m => m.MaritalStatus?.ToString() ?? "Unknown")
            .Select(g => new MaritalStatusBreakdownDto { Status = g.Key, Count = g.Count() })
            .OrderByDescending(g => g.Count)
            .ToList();

        // Age groups — DateOfBirth is DateOnly?
        var ageGroups = new Dictionary<string, int>
        {
            ["Under 18"] = 0,
            ["18–29"] = 0,
            ["30–44"] = 0,
            ["45–59"] = 0,
            ["60+"] = 0,
            ["Unknown"] = 0
        };

        foreach (var m in members)
        {
            if (!m.DateOfBirth.HasValue) { ageGroups["Unknown"]++; continue; }
            var dob = m.DateOfBirth.Value;
            var age = today.Year - dob.Year;
            if (today.DayOfYear < dob.DayOfYear) age--;
            if (age < 18) ageGroups["Under 18"]++;
            else if (age <= 29) ageGroups["18–29"]++;
            else if (age <= 44) ageGroups["30–44"]++;
            else if (age <= 59) ageGroups["45–59"]++;
            else ageGroups["60+"]++;
        }

        var byAgeGroup = ageGroups
            .Select(kv => new AgeGroupDto { Label = kv.Key, Count = kv.Value })
            .ToList();

        // Monthly growth trend — JoinDate is DateOnly?
        var trendStart = now.AddMonths(-request.TrendMonths + 1);
        var trendFrom = DateOnly.FromDateTime(
            new DateTime(trendStart.Year, trendStart.Month, 1));
        var membersBeforeTrend = members.Count(
            m => m.JoinDate.HasValue && m.JoinDate.Value < trendFrom);

        var growth = new List<MonthlyGrowthDto>();
        var cursor = trendFrom;
        var running = membersBeforeTrend;

        for (int i = 0; i < request.TrendMonths; i++)
        {
            var newThisMonth = members.Count(m => m.JoinDate.HasValue
                && m.JoinDate.Value.Year == cursor.Year
                && m.JoinDate.Value.Month == cursor.Month);
            running += newThisMonth;
            growth.Add(new MonthlyGrowthDto
            {
                Year = cursor.Year,
                Month = cursor.Month,
                NewMembers = newThisMonth,
                CumulativeTotal = running
            });
            cursor = cursor.AddMonths(1);
        }

        return ApiResponse<MemberReportDto>.SuccessResult(new MemberReportDto
        {
            Total = members.Count,
            Active = members.Count(m => m.Status == MemberStatus.Active),
            Inactive = members.Count(m => m.Status == MemberStatus.Inactive),
            Deceased = members.Count(m => m.Status == MemberStatus.Deceased),
            Transferred = members.Count(m => m.Status == MemberStatus.Transferred),
            ByGender = byGender,
            ByAgeGroup = byAgeGroup,
            ByMaritalStatus = byMaritalStatus,
            MonthlyGrowth = growth
        });
    }
}
