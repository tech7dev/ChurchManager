using ChurchMS.Application.Features.Messaging.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Messaging.Queries.GetAppointmentList;

public class GetAppointmentListQueryHandler(
    IRepository<Appointment> appointmentRepository,
    IRepository<Member> memberRepository)
    : IRequestHandler<GetAppointmentListQuery, ApiResponse<PagedResult<AppointmentDto>>>
{
    public async Task<ApiResponse<PagedResult<AppointmentDto>>> Handle(
        GetAppointmentListQuery request, CancellationToken cancellationToken)
    {
        var all = await appointmentRepository.FindAsync(
            a => (!request.MemberId.HasValue || a.MemberId == request.MemberId.Value)
              && (!request.ResponsibleMemberId.HasValue || a.ResponsibleMemberId == request.ResponsibleMemberId.Value)
              && (!request.Status.HasValue || a.Status == request.Status.Value),
            cancellationToken);

        var totalCount = all.Count;
        var paged = all
            .OrderByDescending(a => a.RequestedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var dtos = new List<AppointmentDto>();
        foreach (var a in paged)
        {
            var member = await memberRepository.GetByIdAsync(a.MemberId, cancellationToken);
            var responsible = await memberRepository.GetByIdAsync(a.ResponsibleMemberId, cancellationToken);

            dtos.Add(new AppointmentDto
            {
                Id = a.Id,
                ChurchId = a.ChurchId,
                MemberId = a.MemberId,
                MemberName = member is not null ? $"{member.FirstName} {member.LastName}" : "",
                ResponsibleMemberId = a.ResponsibleMemberId,
                ResponsibleName = responsible is not null ? $"{responsible.FirstName} {responsible.LastName}" : "",
                Subject = a.Subject,
                Description = a.Description,
                Status = a.Status,
                MeetingType = a.MeetingType,
                RequestedAt = a.RequestedAt,
                ScheduledAt = a.ScheduledAt,
                CompletedAt = a.CompletedAt,
                VideoCallLink = a.VideoCallLink,
                Location = a.Location,
                Notes = a.Notes,
                CreatedAt = a.CreatedAt
            });
        }

        return ApiResponse<PagedResult<AppointmentDto>>.SuccessResult(new PagedResult<AppointmentDto>
        {
            Items = dtos,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        });
    }
}
