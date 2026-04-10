using ChurchMS.Application.Features.Messaging.Commands.CreateAppointment;
using ChurchMS.Application.Features.Messaging.Commands.CreateMessageCampaign;
using ChurchMS.Application.Features.Messaging.Commands.ScheduleAppointment;
using ChurchMS.Application.Features.Messaging.Commands.SendCampaign;
using ChurchMS.Application.Features.Messaging.DTOs;
using ChurchMS.Application.Features.Messaging.Queries.GetAppointmentList;
using ChurchMS.Application.Features.Messaging.Queries.GetCampaignList;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Constants;
using ChurchMS.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchMS.API.Controllers;

/// <summary>
/// Message campaigns (SMS/Email/WhatsApp) and audience appointments.
/// </summary>
[Authorize]
public class MessagingController : BaseApiController
{
    // ── Campaigns ─────────────────────────────────────────────────────────────

    /// <summary>Get paginated list of message campaigns.</summary>
    [HttpGet("campaigns")]
    [Authorize(Policy = AuthorizationPolicies.RequireSecretary)]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<MessageCampaignDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCampaigns(
        [FromQuery] MessageChannel? channel = null,
        [FromQuery] MessageCampaignStatus? status = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
        => Ok(await Mediator.Send(new GetCampaignListQuery(channel, status, page, pageSize)));

    /// <summary>Create a new message campaign (Draft or Scheduled).</summary>
    [HttpPost("campaigns")]
    [Authorize(Policy = AuthorizationPolicies.RequireSecretary)]
    [ProducesResponseType(typeof(ApiResponse<MessageCampaignDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateCampaign([FromBody] CreateMessageCampaignCommand command)
    {
        var result = await Mediator.Send(command);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    /// <summary>Immediately send a campaign.</summary>
    [HttpPost("campaigns/{campaignId:guid}/send")]
    [Authorize(Policy = AuthorizationPolicies.RequireSecretary)]
    [ProducesResponseType(typeof(ApiResponse<MessageCampaignDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SendCampaign(Guid campaignId)
        => Ok(await Mediator.Send(new SendCampaignCommand(campaignId)));

    // ── Appointments ──────────────────────────────────────────────────────────

    /// <summary>Get paginated list of appointments.</summary>
    [HttpGet("appointments")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<AppointmentDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAppointments(
        [FromQuery] Guid? memberId = null,
        [FromQuery] Guid? responsibleMemberId = null,
        [FromQuery] AppointmentStatus? status = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
        => Ok(await Mediator.Send(new GetAppointmentListQuery(memberId, responsibleMemberId, status, page, pageSize)));

    /// <summary>Request a new appointment.</summary>
    [HttpPost("appointments")]
    [ProducesResponseType(typeof(ApiResponse<AppointmentDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentCommand command)
    {
        var result = await Mediator.Send(command);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    /// <summary>Schedule (confirm date/time for) a pending appointment.</summary>
    [HttpPost("appointments/{appointmentId:guid}/schedule")]
    [Authorize(Policy = AuthorizationPolicies.RequireSecretary)]
    [ProducesResponseType(typeof(ApiResponse<AppointmentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ScheduleAppointment(
        Guid appointmentId,
        [FromBody] ScheduleAppointmentCommand command)
    {
        var cmd = command with { AppointmentId = appointmentId };
        return Ok(await Mediator.Send(cmd));
    }
}
