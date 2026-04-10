using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Application.Exceptions;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Notifications.Commands.MarkNotificationRead;

public class MarkNotificationReadCommandHandler(
    IRepository<Notification> notificationRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<MarkNotificationReadCommand, ApiResponse<bool>>
{
    public async Task<ApiResponse<bool>> Handle(
        MarkNotificationReadCommand request, CancellationToken cancellationToken)
    {
        var notification = await notificationRepository.GetByIdAsync(request.NotificationId, cancellationToken)
            ?? throw new NotFoundException(nameof(Notification), request.NotificationId);

        if (!notification.IsRead)
        {
            notification.IsRead = true;
            notification.ReadAt = DateTime.UtcNow;
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return ApiResponse<bool>.SuccessResult(true);
    }
}
