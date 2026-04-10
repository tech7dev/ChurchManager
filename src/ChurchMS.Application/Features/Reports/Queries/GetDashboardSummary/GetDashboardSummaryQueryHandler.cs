using ChurchMS.Application.Features.Reports.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Enums;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Reports.Queries.GetDashboardSummary;

public class GetDashboardSummaryQueryHandler(
    IRepository<Member> memberRepository,
    IRepository<Family> familyRepository,
    IRepository<Visitor> visitorRepository,
    IRepository<ChurchEvent> eventRepository,
    IRepository<Contribution> contributionRepository,
    IRepository<Expense> expenseRepository,
    IRepository<Department> departmentRepository,
    IRepository<SmsCredit> smsCreditRepository,
    IRepository<Subscription> subscriptionRepository,
    IRepository<SupportTicket> ticketRepository,
    IDateTimeService dateTimeService)
    : IRequestHandler<GetDashboardSummaryQuery, ApiResponse<DashboardSummaryDto>>
{
    public async Task<ApiResponse<DashboardSummaryDto>> Handle(
        GetDashboardSummaryQuery request, CancellationToken cancellationToken)
    {
        var now = dateTimeService.UtcNow;
        var monthStart = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var yearStart = new DateTime(now.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var monthStartDate = DateOnly.FromDateTime(monthStart);
        var yearStartDate = DateOnly.FromDateTime(yearStart);

        // Members
        var allMembers = await memberRepository.GetAllAsync(cancellationToken);
        var families = await familyRepository.GetAllAsync(cancellationToken);
        var visitors = await visitorRepository.GetAllAsync(cancellationToken);

        // Events
        var events = await eventRepository.FindAsync(
            e => e.StartDateTime >= now, cancellationToken);
        var eventsThisMonth = await eventRepository.FindAsync(
            e => e.StartDateTime >= monthStart && e.StartDateTime < monthStart.AddMonths(1),
            cancellationToken);

        // Finance — ContributionDate and ExpenseDate are DateOnly
        var contributionsThisMonth = await contributionRepository.FindAsync(
            c => c.ContributionDate >= monthStartDate && c.Status == ContributionStatus.Confirmed,
            cancellationToken);
        var contributionsThisYear = await contributionRepository.FindAsync(
            c => c.ContributionDate >= yearStartDate && c.Status == ContributionStatus.Confirmed,
            cancellationToken);
        var expensesThisMonth = await expenseRepository.FindAsync(
            e => e.ExpenseDate >= monthStartDate && e.Status != ExpenseStatus.Rejected,
            cancellationToken);

        // Departments, SMS, Subscription, Tickets
        var departments = await departmentRepository.GetAllAsync(cancellationToken);
        var smsWallets = await smsCreditRepository.GetAllAsync(cancellationToken);
        var subscriptions = await subscriptionRepository.FindAsync(
            s => s.Status == SubscriptionStatus.Active || s.Status == SubscriptionStatus.Trial,
            cancellationToken);
        var openTickets = await ticketRepository.FindAsync(
            t => t.Status == TicketStatus.Open || t.Status == TicketStatus.InProgress,
            cancellationToken);

        var activeSubscription = subscriptions.OrderByDescending(s => s.CreatedAt).FirstOrDefault();

        return ApiResponse<DashboardSummaryDto>.SuccessResult(new DashboardSummaryDto
        {
            TotalMembers = allMembers.Count,
            ActiveMembers = allMembers.Count(m => m.Status == MemberStatus.Active),
            NewMembersThisMonth = allMembers.Count(m => m.CreatedAt >= monthStart),
            TotalFamilies = families.Count,
            TotalVisitors = visitors.Count,

            UpcomingEventsCount = events.Count,
            EventsThisMonth = eventsThisMonth.Count,

            ContributionsThisMonth = contributionsThisMonth.Sum(c => c.Amount),
            ContributionsThisYear = contributionsThisYear.Sum(c => c.Amount),
            ExpensesThisMonth = expensesThisMonth.Sum(e => e.Amount),

            TotalDepartments = departments.Count,

            OpenSupportTickets = openTickets.Count,
            SmsCreditBalance = smsWallets.Sum(w => w.Balance),
            HasActiveSubscription = activeSubscription is not null,
            ActiveSubscriptionPlan = activeSubscription?.Plan.ToString()
        });
    }
}
