using ChurchMS.Application.Features.Notifications.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Notifications.Queries.GetNotificationList;

public record GetNotificationListQuery(
    bool UnreadOnly = false,
    int Page = 1,
    int PageSize = 20
) : IRequest<ApiResponse<PagedResult<NotificationDto>>>;
