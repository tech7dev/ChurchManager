using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Notifications.Queries.GetUnreadCount;

public class GetUnreadCountQueryHandler(
    IRepository<Notification> notificationRepository,
    ICurrentUserService currentUserService)
    : IRequestHandler<GetUnreadCountQuery, ApiResponse<int>>
{
    public async Task<ApiResponse<int>> Handle(
        GetUnreadCountQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.GetUserId();
        if (!userId.HasValue)
            return ApiResponse<int>.SuccessResult(0);

        var count = await notificationRepository.CountAsync(
            n => n.UserId == userId.Value && !n.IsRead, cancellationToken);

        return ApiResponse<int>.SuccessResult(count);
    }
}
