using ChurchMS.Application.Features.Notifications.Commands.MarkAllNotificationsRead;
using ChurchMS.Application.Features.Notifications.Commands.MarkNotificationRead;
using ChurchMS.Application.Features.Notifications.DTOs;
using ChurchMS.Application.Features.Notifications.Queries.GetNotificationList;
using ChurchMS.Application.Features.Notifications.Queries.GetUnreadCount;
using ChurchMS.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchMS.API.Controllers;

/// <summary>
/// In-app notifications for the current user.
/// </summary>
[Authorize]
public class NotificationsController : BaseApiController
{
    /// <summary>Get notifications for the current user.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<NotificationDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetList(
        [FromQuery] bool unreadOnly = false,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
        => Ok(await Mediator.Send(new GetNotificationListQuery(unreadOnly, page, pageSize)));

    /// <summary>Get the unread notification count for the current user.</summary>
    [HttpGet("unread-count")]
    [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUnreadCount()
        => Ok(await Mediator.Send(new GetUnreadCountQuery()));

    /// <summary>Mark a single notification as read.</summary>
    [HttpPost("{id:guid}/read")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> MarkRead(Guid id)
        => Ok(await Mediator.Send(new MarkNotificationReadCommand(id)));

    /// <summary>Mark all notifications as read for the current user.</summary>
    [HttpPost("read-all")]
    [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
    public async Task<IActionResult> MarkAllRead()
        => Ok(await Mediator.Send(new MarkAllNotificationsReadCommand()));
}
