using ChurchMS.Application.Features.Notifications.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Notifications.Queries.GetNotificationList;

public class GetNotificationListQueryHandler(
    IRepository<Notification> notificationRepository,
    ICurrentUserService currentUserService)
    : IRequestHandler<GetNotificationListQuery, ApiResponse<PagedResult<NotificationDto>>>
{
    public async Task<ApiResponse<PagedResult<NotificationDto>>> Handle(
        GetNotificationListQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.GetUserId();
        if (!userId.HasValue)
            return ApiResponse<PagedResult<NotificationDto>>.SuccessResult(new PagedResult<NotificationDto>
            {
                Items = [],
                TotalCount = 0,
                Page = request.Page,
                PageSize = request.PageSize
            });

        var all = request.UnreadOnly
            ? await notificationRepository.FindAsync(n => n.UserId == userId.Value && !n.IsRead, cancellationToken)
            : await notificationRepository.FindAsync(n => n.UserId == userId.Value, cancellationToken);

        var totalCount = all.Count;
        var paged = all
            .OrderByDescending(n => n.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var dtos = paged.Select(n => new NotificationDto
        {
            Id = n.Id,
            ChurchId = n.ChurchId,
            UserId = n.UserId,
            Title = n.Title,
            Body = n.Body,
            Type = n.Type,
            IsRead = n.IsRead,
            ReadAt = n.ReadAt,
            RelatedEntityType = n.RelatedEntityType,
            RelatedEntityId = n.RelatedEntityId,
            ActionUrl = n.ActionUrl,
            CreatedAt = n.CreatedAt
        }).ToList();

        return ApiResponse<PagedResult<NotificationDto>>.SuccessResult(new PagedResult<NotificationDto>
        {
            Items = dtos,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        });
    }
}
