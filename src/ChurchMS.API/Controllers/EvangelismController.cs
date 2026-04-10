using ChurchMS.Application.Features.Evangelism.Commands.AddContact;
using ChurchMS.Application.Features.Evangelism.Commands.AssignTeamMember;
using ChurchMS.Application.Features.Evangelism.Commands.CreateCampaign;
using ChurchMS.Application.Features.Evangelism.Commands.CreateTeam;
using ChurchMS.Application.Features.Evangelism.Commands.RecordFollowUp;
using ChurchMS.Application.Features.Evangelism.Commands.UpdateContactStatus;
using ChurchMS.Application.Features.Evangelism.DTOs;
using ChurchMS.Application.Features.Evangelism.Queries.GetCampaignById;
using ChurchMS.Application.Features.Evangelism.Queries.GetCampaignList;
using ChurchMS.Application.Features.Evangelism.Queries.GetContactList;
using ChurchMS.Application.Features.Evangelism.Queries.GetFollowUpList;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchMS.API.Controllers;

/// <summary>
/// Evangelism module: campaigns, teams, contacts, and follow-up tracking.
/// </summary>
[Authorize]
public class EvangelismController : BaseApiController
{
    // ── Campaigns ─────────────────────────────────────────────────────────

    /// <summary>List evangelism campaigns, optionally filtered by status.</summary>
    [HttpGet("campaigns")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<EvangelismCampaignDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCampaigns(
        [FromQuery] EvangelismCampaignStatus? status = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
        => Ok(await Mediator.Send(new GetCampaignListQuery(status, page, pageSize)));

    /// <summary>Get a single evangelism campaign by ID.</summary>
    [HttpGet("campaigns/{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<EvangelismCampaignDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCampaignById(Guid id)
        => Ok(await Mediator.Send(new GetCampaignByIdQuery(id)));

    /// <summary>Create a new evangelism campaign.</summary>
    [HttpPost("campaigns")]
    [Authorize(Policy = "EvangelismLeaderOrAbove")]
    [ProducesResponseType(typeof(ApiResponse<EvangelismCampaignDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateCampaign([FromBody] CreateCampaignCommand command)
    {
        var result = await Mediator.Send(command);
        return result.Success ? StatusCode(StatusCodes.Status201Created, result) : BadRequest(result);
    }

    // ── Teams ─────────────────────────────────────────────────────────────

    /// <summary>Create a team within a campaign.</summary>
    [HttpPost("campaigns/{campaignId:guid}/teams")]
    [Authorize(Policy = "EvangelismLeaderOrAbove")]
    [ProducesResponseType(typeof(ApiResponse<EvangelismTeamDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateTeam(Guid campaignId, [FromBody] CreateTeamRequest request)
    {
        var command = new CreateTeamCommand(campaignId, request.Name, request.LeaderMemberId, request.Notes);
        var result = await Mediator.Send(command);
        return result.Success ? StatusCode(StatusCodes.Status201Created, result) : BadRequest(result);
    }

    /// <summary>Assign a member to a team.</summary>
    [HttpPost("teams/{teamId:guid}/members")]
    [Authorize(Policy = "EvangelismLeaderOrAbove")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> AssignTeamMember(Guid teamId, [FromBody] AssignTeamMemberRequest request)
    {
        var command = new AssignTeamMemberCommand(teamId, request.MemberId, request.JoinedDate);
        return Ok(await Mediator.Send(command));
    }

    // ── Contacts ──────────────────────────────────────────────────────────

    /// <summary>List contacts, optionally filtered by campaign, team, or status.</summary>
    [HttpGet("contacts")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<EvangelismContactDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetContacts(
        [FromQuery] Guid? campaignId = null,
        [FromQuery] Guid? teamId = null,
        [FromQuery] ContactStatus? status = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
        => Ok(await Mediator.Send(new GetContactListQuery(campaignId, teamId, status, page, pageSize)));

    /// <summary>Add a new contact to a campaign.</summary>
    [HttpPost("contacts")]
    [ProducesResponseType(typeof(ApiResponse<EvangelismContactDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> AddContact([FromBody] AddContactCommand command)
    {
        var result = await Mediator.Send(command);
        return result.Success ? StatusCode(StatusCodes.Status201Created, result) : BadRequest(result);
    }

    /// <summary>Update the status of an evangelism contact.</summary>
    [HttpPut("contacts/{contactId:guid}/status")]
    [ProducesResponseType(typeof(ApiResponse<EvangelismContactDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateContactStatus(Guid contactId, [FromBody] UpdateContactStatusRequest request)
    {
        var command = new UpdateContactStatusCommand(contactId, request.Status, request.ConvertedMemberId);
        return Ok(await Mediator.Send(command));
    }

    // ── Follow-Ups ────────────────────────────────────────────────────────

    /// <summary>List follow-up interactions for a contact.</summary>
    [HttpGet("contacts/{contactId:guid}/follow-ups")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<EvangelismFollowUpDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFollowUps(
        Guid contactId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
        => Ok(await Mediator.Send(new GetFollowUpListQuery(contactId, page, pageSize)));

    /// <summary>Record a follow-up interaction for a contact.</summary>
    [HttpPost("contacts/{contactId:guid}/follow-ups")]
    [ProducesResponseType(typeof(ApiResponse<EvangelismFollowUpDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> RecordFollowUp(Guid contactId, [FromBody] RecordFollowUpRequest request)
    {
        var command = new RecordFollowUpCommand(contactId, request.Method, request.FollowUpDate, request.Notes, request.ConductedByMemberId);
        var result = await Mediator.Send(command);
        return result.Success ? StatusCode(StatusCodes.Status201Created, result) : BadRequest(result);
    }
}

// ── Request body models ───────────────────────────────────────────────────

public record CreateTeamRequest(string Name, Guid? LeaderMemberId, string? Notes);
public record AssignTeamMemberRequest(Guid MemberId, DateOnly JoinedDate);
public record UpdateContactStatusRequest(ContactStatus Status, Guid? ConvertedMemberId);
public record RecordFollowUpRequest(FollowUpMethod Method, DateOnly FollowUpDate, string? Notes, Guid? ConductedByMemberId);
