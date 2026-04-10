using ChurchMS.Application.Features.Members.Commands.CreateCustomField;
using ChurchMS.Application.Features.Members.Commands.CreateFamily;
using ChurchMS.Application.Features.Members.Commands.CreateMember;
using ChurchMS.Application.Features.Members.Commands.DeleteMember;
using ChurchMS.Application.Features.Members.Commands.SetMemberCustomFieldValue;
using ChurchMS.Application.Features.Members.Commands.UpdateMember;
using ChurchMS.Application.Features.Members.DTOs;
using ChurchMS.Application.Features.Members.Queries.GetCustomFields;
using ChurchMS.Application.Features.Members.Queries.GetFamilyById;
using ChurchMS.Application.Features.Members.Queries.GetMemberById;
using ChurchMS.Application.Features.Members.Queries.GetMemberList;
using ChurchMS.Shared.Constants;
using ChurchMS.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchMS.API.Controllers;

[Authorize]
public class MembersController : BaseApiController
{
    // ── Members ──────────────────────────────────────────────────────────

    /// <summary>
    /// Get a paginated list of members with optional search and status filter.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<MemberListDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetList(
        [FromQuery] string? search,
        [FromQuery] string? status,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Mediator.Send(new GetMemberListQuery
        {
            SearchTerm = search,
            Status = status,
            Page = page,
            PageSize = pageSize
        });
        return Ok(result);
    }

    /// <summary>
    /// Get a member by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<MemberDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<MemberDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await Mediator.Send(new GetMemberByIdQuery(id));
        return result.Success ? Ok(result) : NotFound(result);
    }

    /// <summary>
    /// Create a new member.
    /// </summary>
    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.RequireSecretary)]
    [ProducesResponseType(typeof(ApiResponse<MemberDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<MemberDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateMemberCommand command)
    {
        var result = await Mediator.Send(command);
        return result.Success
            ? StatusCode(StatusCodes.Status201Created, result)
            : BadRequest(result);
    }

    /// <summary>
    /// Update an existing member.
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize(Policy = AuthorizationPolicies.RequireSecretary)]
    [ProducesResponseType(typeof(ApiResponse<MemberDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<MemberDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateMemberCommand command)
    {
        command.Id = id;
        var result = await Mediator.Send(command);
        return result.Success ? Ok(result) : NotFound(result);
    }

    /// <summary>
    /// Soft-delete a member.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Policy = AuthorizationPolicies.RequireChurchAdmin)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await Mediator.Send(new DeleteMemberCommand(id));
        return result.Success ? NoContent() : NotFound(result);
    }

    // ── Custom field values ────────────────────────────────────────────

    /// <summary>
    /// Set a custom field value on a member.
    /// </summary>
    [HttpPost("{id:guid}/custom-fields")]
    [Authorize(Policy = AuthorizationPolicies.RequireSecretary)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SetCustomFieldValue(Guid id, [FromBody] SetMemberCustomFieldValueCommand command)
    {
        command.MemberId = id;
        var result = await Mediator.Send(command);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    // ── Families ──────────────────────────────────────────────────────

    /// <summary>
    /// Get a family by ID.
    /// </summary>
    [HttpGet("families/{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<FamilyDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<FamilyDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFamilyById(Guid id)
    {
        var result = await Mediator.Send(new GetFamilyByIdQuery(id));
        return result.Success ? Ok(result) : NotFound(result);
    }

    /// <summary>
    /// Create a new family.
    /// </summary>
    [HttpPost("families")]
    [Authorize(Policy = AuthorizationPolicies.RequireSecretary)]
    [ProducesResponseType(typeof(ApiResponse<FamilyDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateFamily([FromBody] CreateFamilyCommand command)
    {
        var result = await Mediator.Send(command);
        return result.Success
            ? StatusCode(StatusCodes.Status201Created, result)
            : BadRequest(result);
    }

    // ── Custom field definitions ───────────────────────────────────────

    /// <summary>
    /// Get all custom fields for the current church.
    /// </summary>
    [HttpGet("custom-fields")]
    [ProducesResponseType(typeof(ApiResponse<List<CustomFieldDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCustomFields()
    {
        var result = await Mediator.Send(new GetCustomFieldsQuery());
        return Ok(result);
    }

    /// <summary>
    /// Create a new custom field definition.
    /// </summary>
    [HttpPost("custom-fields")]
    [Authorize(Policy = AuthorizationPolicies.RequireChurchAdmin)]
    [ProducesResponseType(typeof(ApiResponse<CustomFieldDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateCustomField([FromBody] CreateCustomFieldCommand command)
    {
        var result = await Mediator.Send(command);
        return result.Success
            ? StatusCode(StatusCodes.Status201Created, result)
            : BadRequest(result);
    }
}
