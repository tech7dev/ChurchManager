using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Notifications.Commands.MarkAllNotificationsRead;

public record MarkAllNotificationsReadCommand : IRequest<ApiResponse<int>>;
