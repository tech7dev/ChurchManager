using ChurchMS.Application.Features.Events.Commands.CancelEvent;
using ChurchMS.Application.Features.Events.Commands.CreateEvent;
using ChurchMS.Application.Features.Events.Commands.PublishEvent;
using ChurchMS.Application.Features.Events.Commands.RecordAttendance;
using ChurchMS.Application.Features.Events.Commands.RegisterForEvent;
using ChurchMS.Application.Features.Events.DTOs;
using ChurchMS.Application.Features.Events.Queries.GetEventAttendance;
using ChurchMS.Application.Features.Events.Queries.GetEventById;
using ChurchMS.Application.Features.Events.Queries.GetEventList;
using ChurchMS.Application.Features.Events.Queries.GetEventRegistrations;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Constants;
using ChurchMS.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchMS.API.Controllers;

/// <summary>
/// Events, Registrations, and Attendance.
/// </summary>
[Authorize]
public class EventsController : BaseApiController
{
    // ── Events ────────────────────────────────────────────────────────────────

    /// <summary>Get paginated list of events.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<EventListDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetList(
        [FromQuery] EventType? type = null,
        [FromQuery] EventStatus? status = null,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
        => Ok(await Mediator.Send(new GetEventListQuery(type, status, fromDate, toDate, page, pageSize)));

    /// <summary>Get event details by ID.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<EventDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
        => Ok(await Mediator.Send(new GetEventByIdQuery(id)));

    /// <summary>Create a new event (draft).</summary>
    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.RequireSecretary)]
    [ProducesResponseType(typeof(ApiResponse<EventDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateEventCommand command)
    {
        var result = await Mediator.Send(command);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    /// <summary>Publish a draft event.</summary>
    [HttpPost("{id:guid}/publish")]
    [Authorize(Policy = AuthorizationPolicies.RequireSecretary)]
    [ProducesResponseType(typeof(ApiResponse<EventDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Publish(Guid id)
        => Ok(await Mediator.Send(new PublishEventCommand(id)));

    /// <summary>Cancel an event.</summary>
    [HttpPost("{id:guid}/cancel")]
    [Authorize(Policy = AuthorizationPolicies.RequireSecretary)]
    [ProducesResponseType(typeof(ApiResponse<EventDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Cancel(Guid id)
        => Ok(await Mediator.Send(new CancelEventCommand(id)));

    // ── Registrations ─────────────────────────────────────────────────────────

    /// <summary>Get registrations for an event.</summary>
    [HttpGet("{eventId:guid}/registrations")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<EventRegistrationDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRegistrations(
        Guid eventId,
        [FromQuery] RegistrationStatus? status = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50)
        => Ok(await Mediator.Send(new GetEventRegistrationsQuery(eventId, status, page, pageSize)));

    /// <summary>Register a member or guest for an event.</summary>
    [HttpPost("{eventId:guid}/registrations")]
    [ProducesResponseType(typeof(ApiResponse<EventRegistrationDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Register(Guid eventId, [FromBody] RegisterForEventCommand command)
    {
        var cmd = command with { EventId = eventId };
        var result = await Mediator.Send(cmd);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    // ── Attendance ────────────────────────────────────────────────────────────

    /// <summary>Get attendance records for an event.</summary>
    [HttpGet("{eventId:guid}/attendance")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<EventAttendanceDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAttendance(
        Guid eventId,
        [FromQuery] DateOnly? date = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 100)
        => Ok(await Mediator.Send(new GetEventAttendanceQuery(eventId, date, page, pageSize)));

    /// <summary>Record attendance for an event.</summary>
    [HttpPost("{eventId:guid}/attendance")]
    [ProducesResponseType(typeof(ApiResponse<EventAttendanceDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> RecordAttendance(
        Guid eventId,
        [FromBody] RecordAttendanceCommand command)
    {
        var cmd = command with { EventId = eventId };
        var result = await Mediator.Send(cmd);
        return StatusCode(StatusCodes.Status201Created, result);
    }
}
