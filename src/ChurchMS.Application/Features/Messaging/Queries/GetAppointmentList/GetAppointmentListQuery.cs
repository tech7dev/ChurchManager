using ChurchMS.Application.Features.Messaging.DTOs;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Messaging.Queries.GetAppointmentList;

public record GetAppointmentListQuery(
    Guid? MemberId = null,
    Guid? ResponsibleMemberId = null,
    AppointmentStatus? Status = null,
    int Page = 1,
    int PageSize = 20
) : IRequest<ApiResponse<PagedResult<AppointmentDto>>>;
