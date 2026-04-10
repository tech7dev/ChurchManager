using ChurchMS.Application.Features.ITManagement.Commands.AddTicketComment;
using ChurchMS.Application.Features.ITManagement.Commands.AssignUserRole;
using ChurchMS.Application.Features.ITManagement.Commands.CreateIntegrationConfig;
using ChurchMS.Application.Features.ITManagement.Commands.CreateSupportTicket;
using ChurchMS.Application.Features.ITManagement.Commands.UpdateIntegrationConfig;
using ChurchMS.Application.Features.ITManagement.Commands.UpdateTicketStatus;
using ChurchMS.Application.Features.ITManagement.Commands.UpdateUserStatus;
using ChurchMS.Application.Features.ITManagement.Queries.GetIntegrationConfigList;
using ChurchMS.Application.Features.ITManagement.Queries.GetSupportTicketById;
using ChurchMS.Application.Features.ITManagement.Queries.GetSupportTicketList;
using ChurchMS.Application.Features.ITManagement.Queries.GetSystemLogList;
using ChurchMS.Application.Features.ITManagement.Queries.GetUserById;
using ChurchMS.Application.Features.ITManagement.Queries.GetUserList;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchMS.API.Controllers;

/// <summary>
/// IT Management: user management, support tickets, integration configuration, system logs.
/// </summary>
[Authorize(Policy = AuthorizationPolicies.RequireITManager)]
public class ITManagementController : BaseApiController
{
    // ─── Users ───────────────────────────────────────────────────────────

    /// <summary>List users in the current church (or all churches for SuperAdmin).</summary>
    [HttpGet("users")]
    public async Task<IActionResult> GetUsers(
        [FromQuery] bool? isActive = null,
        [FromQuery] string? role = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
        => Ok(await Mediator.Send(new GetUserListQuery(isActive, role, page, pageSize)));

    /// <summary>Get a single user by ID.</summary>
    [HttpGet("users/{id:guid}")]
    public async Task<IActionResult> GetUserById(Guid id)
        => Ok(await Mediator.Send(new GetUserByIdQuery(id)));

    /// <summary>Activate or deactivate a user account.</summary>
    [HttpPut("users/{id:guid}/status")]
    public async Task<IActionResult> UpdateUserStatus(Guid id, [FromBody] UpdateUserStatusCommand command)
    {
        if (id != command.UserId)
            return BadRequest("Route ID and body ID mismatch.");
        return Ok(await Mediator.Send(command));
    }

    /// <summary>Assign a role to a user (replaces the current role).</summary>
    [HttpPut("users/{id:guid}/role")]
    public async Task<IActionResult> AssignRole(Guid id, [FromBody] AssignUserRoleCommand command)
    {
        if (id != command.UserId)
            return BadRequest("Route ID and body ID mismatch.");
        return Ok(await Mediator.Send(command));
    }

    // ─── Support Tickets ─────────────────────────────────────────────────

    /// <summary>List support tickets with optional filters.</summary>
    [HttpGet("tickets")]
    public async Task<IActionResult> GetTickets(
        [FromQuery] TicketStatus? status = null,
        [FromQuery] TicketPriority? priority = null,
        [FromQuery] TicketCategory? category = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
        => Ok(await Mediator.Send(new GetSupportTicketListQuery(status, priority, category, page, pageSize)));

    /// <summary>Get a support ticket with its comments.</summary>
    [HttpGet("tickets/{id:guid}")]
    public async Task<IActionResult> GetTicketById(Guid id)
        => Ok(await Mediator.Send(new GetSupportTicketByIdQuery(id)));

    /// <summary>Submit a new support ticket.</summary>
    [HttpPost("tickets")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateTicket([FromBody] CreateSupportTicketCommand command)
    {
        var result = await Mediator.Send(command);
        return result.Success ? StatusCode(StatusCodes.Status201Created, result) : BadRequest(result);
    }

    /// <summary>Update ticket status, assignment, or resolution notes.</summary>
    [HttpPut("tickets/{id:guid}/status")]
    public async Task<IActionResult> UpdateTicketStatus(Guid id, [FromBody] UpdateTicketStatusCommand command)
    {
        if (id != command.TicketId)
            return BadRequest("Route ID and body ID mismatch.");
        return Ok(await Mediator.Send(command));
    }

    /// <summary>Add a comment to a support ticket.</summary>
    [HttpPost("tickets/{id:guid}/comments")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> AddComment(Guid id, [FromBody] AddTicketCommentCommand command)
    {
        if (id != command.TicketId)
            return BadRequest("Route ID and body ID mismatch.");
        var result = await Mediator.Send(command);
        return result.Success ? StatusCode(StatusCodes.Status201Created, result) : BadRequest(result);
    }

    // ─── Integration Config ───────────────────────────────────────────────

    /// <summary>List integration configurations for the current church.</summary>
    [HttpGet("integrations")]
    public async Task<IActionResult> GetIntegrations(
        [FromQuery] IntegrationService? service = null)
        => Ok(await Mediator.Send(new GetIntegrationConfigListQuery(service)));

    /// <summary>Create a new integration configuration.</summary>
    [HttpPost("integrations")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateIntegration([FromBody] CreateIntegrationConfigCommand command)
    {
        var result = await Mediator.Send(command);
        return result.Success ? StatusCode(StatusCodes.Status201Created, result) : BadRequest(result);
    }

    /// <summary>Update an existing integration configuration.</summary>
    [HttpPut("integrations/{id:guid}")]
    public async Task<IActionResult> UpdateIntegration(Guid id, [FromBody] UpdateIntegrationConfigCommand command)
    {
        if (id != command.ConfigId)
            return BadRequest("Route ID and body ID mismatch.");
        return Ok(await Mediator.Send(command));
    }

    // ─── System Logs ──────────────────────────────────────────────────────

    /// <summary>Query system audit logs with optional filters.</summary>
    [HttpGet("logs")]
    public async Task<IActionResult> GetLogs(
        [FromQuery] string? action = null,
        [FromQuery] string? entityType = null,
        [FromQuery] string? level = null,
        [FromQuery] DateTime? from = null,
        [FromQuery] DateTime? to = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50)
        => Ok(await Mediator.Send(new GetSystemLogListQuery(action, entityType, level, from, to, page, pageSize)));
}
