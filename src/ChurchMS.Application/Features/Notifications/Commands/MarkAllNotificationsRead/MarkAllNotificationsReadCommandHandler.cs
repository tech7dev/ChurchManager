using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Notifications.Commands.MarkAllNotificationsRead;

public class MarkAllNotificationsReadCommandHandler(
    IRepository<Notification> notificationRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<MarkAllNotificationsReadCommand, ApiResponse<int>>
{
    public async Task<ApiResponse<int>> Handle(
        MarkAllNotificationsReadCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.GetUserId();
        if (!userId.HasValue)
            return ApiResponse<int>.SuccessResult(0);

        var unread = await notificationRepository.FindAsync(
            n => n.UserId == userId.Value && !n.IsRead, cancellationToken);

        var now = DateTime.UtcNow;
        foreach (var notification in unread)
        {
            notification.IsRead = true;
            notification.ReadAt = now;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return ApiResponse<int>.SuccessResult(unread.Count);
    }
}
