using ChurchMS.Application.Features.Contributions.Commands.CreateCampaign;
using ChurchMS.Application.Features.Contributions.Commands.CreateContribution;
using ChurchMS.Application.Features.Contributions.Commands.CreateFund;
using ChurchMS.Application.Features.Contributions.Commands.CreatePledge;
using ChurchMS.Application.Features.Contributions.DTOs;
using ChurchMS.Application.Features.Contributions.Queries.GetCampaignList;
using ChurchMS.Application.Features.Contributions.Queries.GetContributionById;
using ChurchMS.Application.Features.Contributions.Queries.GetContributionList;
using ChurchMS.Application.Features.Contributions.Queries.GetContributionSummary;
using ChurchMS.Application.Features.Contributions.Queries.GetFundList;
using ChurchMS.Application.Features.Contributions.Queries.GetPledgeList;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Constants;
using ChurchMS.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchMS.API.Controllers;

/// <summary>
/// Finance: Contributions, Funds, Campaigns, and Pledges.
/// </summary>
[Authorize]
public class ContributionsController : BaseApiController
{
    // ── Funds ────────────────────────────────────────────────────────────────

    /// <summary>Get list of giving funds.</summary>
    [HttpGet("funds")]
    [ProducesResponseType(typeof(ApiResponse<IList<FundDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFunds([FromQuery] bool activeOnly = true)
        => Ok(await Mediator.Send(new GetFundListQuery(activeOnly)));

    /// <summary>Create a new fund.</summary>
    [HttpPost("funds")]
    [Authorize(Policy = AuthorizationPolicies.RequireTreasurer)]
    [ProducesResponseType(typeof(ApiResponse<FundDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateFund([FromBody] CreateFundCommand command)
    {
        var result = await Mediator.Send(command);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    // ── Contributions ────────────────────────────────────────────────────────

    /// <summary>Get paginated list of contributions.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<ContributionListDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetList(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] Guid? fundId = null,
        [FromQuery] Guid? campaignId = null,
        [FromQuery] Guid? memberId = null,
        [FromQuery] ContributionType? type = null,
        [FromQuery] ContributionStatus? status = null,
        [FromQuery] DateOnly? fromDate = null,
        [FromQuery] DateOnly? toDate = null)
        => Ok(await Mediator.Send(new GetContributionListQuery(
            page, pageSize, null, fundId, campaignId, memberId, type, status, fromDate, toDate)));

    /// <summary>Get a contribution by ID.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<ContributionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
        => Ok(await Mediator.Send(new GetContributionByIdQuery(id)));

    /// <summary>Record a new contribution.</summary>
    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.RequireTreasurer)]
    [ProducesResponseType(typeof(ApiResponse<ContributionDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateContributionCommand command)
    {
        var result = await Mediator.Send(command);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    /// <summary>Get contribution summary (totals by fund and month).</summary>
    [HttpGet("summary")]
    [ProducesResponseType(typeof(ApiResponse<ContributionSummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSummary(
        [FromQuery] int? year = null,
        [FromQuery] int? month = null,
        [FromQuery] Guid? fundId = null)
        => Ok(await Mediator.Send(new GetContributionSummaryQuery(year, month, fundId)));

    // ── Campaigns ────────────────────────────────────────────────────────────

    /// <summary>Get list of fundraising campaigns.</summary>
    [HttpGet("campaigns")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<CampaignDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCampaigns(
        [FromQuery] CampaignStatus? status = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
        => Ok(await Mediator.Send(new GetCampaignListQuery(status, page, pageSize)));

    /// <summary>Create a new campaign.</summary>
    [HttpPost("campaigns")]
    [Authorize(Policy = AuthorizationPolicies.RequireTreasurer)]
    [ProducesResponseType(typeof(ApiResponse<CampaignDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateCampaign([FromBody] CreateCampaignCommand command)
    {
        var result = await Mediator.Send(command);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    // ── Pledges ───────────────────────────────────────────────────────────────

    /// <summary>Get list of pledges.</summary>
    [HttpGet("pledges")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<PledgeDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPledges(
        [FromQuery] Guid? memberId = null,
        [FromQuery] Guid? fundId = null,
        [FromQuery] PledgeStatus? status = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
        => Ok(await Mediator.Send(new GetPledgeListQuery(memberId, fundId, status, page, pageSize)));

    /// <summary>Create a new pledge.</summary>
    [HttpPost("pledges")]
    [Authorize(Policy = AuthorizationPolicies.RequireTreasurer)]
    [ProducesResponseType(typeof(ApiResponse<PledgeDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreatePledge([FromBody] CreatePledgeCommand command)
    {
        var result = await Mediator.Send(command);
        return StatusCode(StatusCodes.Status201Created, result);
    }
}
