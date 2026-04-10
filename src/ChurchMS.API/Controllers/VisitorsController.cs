using ChurchMS.Application.Features.Members.Commands.ConvertVisitorToMember;
using ChurchMS.Application.Features.Members.Commands.CreateVisitor;
using ChurchMS.Application.Features.Members.DTOs;
using ChurchMS.Application.Features.Members.Queries.GetVisitorList;
using ChurchMS.Shared.Constants;
using ChurchMS.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchMS.API.Controllers;

[Authorize]
public class VisitorsController : BaseApiController
{
    /// <summary>
    /// Get a paginated list of visitors.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<VisitorDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetList(
        [FromQuery] string? search,
        [FromQuery] string? status,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Mediator.Send(new GetVisitorListQuery
        {
            SearchTerm = search,
            Status = status,
            Page = page,
            PageSize = pageSize
        });
        return Ok(result);
    }

    /// <summary>
    /// Record a new visitor.
    /// </summary>
    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.RequireSecretary)]
    [ProducesResponseType(typeof(ApiResponse<VisitorDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<VisitorDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateVisitorCommand command)
    {
        var result = await Mediator.Send(command);
        return result.Success
            ? StatusCode(StatusCodes.Status201Created, result)
            : BadRequest(result);
    }

    /// <summary>
    /// Convert a visitor into a full member.
    /// </summary>
    [HttpPost("{id:guid}/convert")]
    [Authorize(Policy = AuthorizationPolicies.RequireSecretary)]
    [ProducesResponseType(typeof(ApiResponse<MemberDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<MemberDto>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<MemberDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Convert(Guid id)
    {
        var result = await Mediator.Send(new ConvertVisitorToMemberCommand(id));
        return result.Success ? Ok(result) : BadRequest(result);
    }
}
