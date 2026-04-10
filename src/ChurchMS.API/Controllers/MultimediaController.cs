using ChurchMS.Application.Features.Multimedia.Commands.ActivateMediaAccess;
using ChurchMS.Application.Features.Multimedia.Commands.CreateMediaContent;
using ChurchMS.Application.Features.Multimedia.Commands.CreatePromotion;
using ChurchMS.Application.Features.Multimedia.Commands.PublishMediaContent;
using ChurchMS.Application.Features.Multimedia.Commands.PurchaseMediaContent;
using ChurchMS.Application.Features.Multimedia.DTOs;
using ChurchMS.Application.Features.Multimedia.Queries.GetMediaContentById;
using ChurchMS.Application.Features.Multimedia.Queries.GetMediaContentList;
using ChurchMS.Application.Features.Multimedia.Queries.GetMediaPurchaseList;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchMS.API.Controllers;

/// <summary>
/// Multimedia module: content library, purchases, and promotions.
/// </summary>
[Authorize]
public class MultimediaController : BaseApiController
{
    // ── Content ───────────────────────────────────────────────────────────

    /// <summary>List media content, optionally filtered by type, status, or access type.</summary>
    [HttpGet("content")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<MediaContentDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetContent(
        [FromQuery] MediaContentType? contentType = null,
        [FromQuery] MediaContentStatus? status = null,
        [FromQuery] MediaAccessType? accessType = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
        => Ok(await Mediator.Send(new GetMediaContentListQuery(contentType, status, accessType, page, pageSize)));

    /// <summary>Get a single media content item by ID.</summary>
    [HttpGet("content/{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<MediaContentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetContentById(Guid id)
        => Ok(await Mediator.Send(new GetMediaContentByIdQuery(id)));

    /// <summary>Create a new media content item (starts as Draft).</summary>
    [HttpPost("content")]
    [Authorize(Policy = "MultimediaManagerOrAbove")]
    [ProducesResponseType(typeof(ApiResponse<MediaContentDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateContent([FromBody] CreateMediaContentCommand command)
    {
        var result = await Mediator.Send(command);
        return result.Success ? StatusCode(StatusCodes.Status201Created, result) : BadRequest(result);
    }

    /// <summary>Publish a media content item (Draft → Published).</summary>
    [HttpPost("content/{id:guid}/publish")]
    [Authorize(Policy = "MultimediaManagerOrAbove")]
    [ProducesResponseType(typeof(ApiResponse<MediaContentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> PublishContent(Guid id)
        => Ok(await Mediator.Send(new PublishMediaContentCommand(id)));

    // ── Purchases ─────────────────────────────────────────────────────────

    /// <summary>List purchases, optionally filtered by content, member, or status.</summary>
    [HttpGet("purchases")]
    [Authorize(Policy = "MultimediaManagerOrAbove")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<MediaPurchaseDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPurchases(
        [FromQuery] Guid? contentId = null,
        [FromQuery] Guid? memberId = null,
        [FromQuery] MediaPurchaseStatus? status = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
        => Ok(await Mediator.Send(new GetMediaPurchaseListQuery(contentId, memberId, status, page, pageSize)));

    /// <summary>Purchase a paid content item (online payment or cash registration).</summary>
    [HttpPost("purchases")]
    [ProducesResponseType(typeof(ApiResponse<MediaPurchaseDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Purchase([FromBody] PurchaseMediaContentCommand command)
    {
        var result = await Mediator.Send(command);
        return result.Success ? StatusCode(StatusCodes.Status201Created, result) : BadRequest(result);
    }

    /// <summary>Manually activate a pending cash purchase.</summary>
    [HttpPost("purchases/{purchaseId:guid}/activate")]
    [Authorize(Policy = "MultimediaManagerOrAbove")]
    [ProducesResponseType(typeof(ApiResponse<MediaPurchaseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ActivateAccess(Guid purchaseId, [FromBody] ActivateAccessRequest request)
    {
        var command = new ActivateMediaAccessCommand(purchaseId, request.ActivatedByMemberId);
        return Ok(await Mediator.Send(command));
    }

    // ── Promotions ────────────────────────────────────────────────────────

    /// <summary>Create a promotional discount for one or all content items.</summary>
    [HttpPost("promotions")]
    [Authorize(Policy = "MultimediaManagerOrAbove")]
    [ProducesResponseType(typeof(ApiResponse<MediaPromotionDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreatePromotion([FromBody] CreatePromotionCommand command)
    {
        var result = await Mediator.Send(command);
        return result.Success ? StatusCode(StatusCodes.Status201Created, result) : BadRequest(result);
    }
}

// ── Request body models ───────────────────────────────────────────────────

public record ActivateAccessRequest(Guid ActivatedByMemberId);
