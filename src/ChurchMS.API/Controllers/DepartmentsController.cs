using ChurchMS.Application.Features.Departments.Commands.AssignMember;
using ChurchMS.Application.Features.Departments.Commands.CreateDepartment;
using ChurchMS.Application.Features.Departments.Commands.RecordDepartmentTransaction;
using ChurchMS.Application.Features.Departments.Commands.RemoveMember;
using ChurchMS.Application.Features.Departments.DTOs;
using ChurchMS.Application.Features.Departments.Queries.GetDepartmentById;
using ChurchMS.Application.Features.Departments.Queries.GetDepartmentList;
using ChurchMS.Application.Features.Departments.Queries.GetDepartmentMembers;
using ChurchMS.Application.Features.Departments.Queries.GetDepartmentTransactions;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Constants;
using ChurchMS.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchMS.API.Controllers;

/// <summary>
/// Ministry departments, membership, and department finances.
/// </summary>
[Authorize]
public class DepartmentsController : BaseApiController
{
    // ── Departments ───────────────────────────────────────────────────────────

    /// <summary>Get all departments.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<List<DepartmentDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetList([FromQuery] bool activeOnly = true)
        => Ok(await Mediator.Send(new GetDepartmentListQuery(activeOnly)));

    /// <summary>Get department details by ID.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<DepartmentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
        => Ok(await Mediator.Send(new GetDepartmentByIdQuery(id)));

    /// <summary>Create a new department.</summary>
    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.RequireChurchAdmin)]
    [ProducesResponseType(typeof(ApiResponse<DepartmentDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateDepartmentCommand command)
    {
        var result = await Mediator.Send(command);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    // ── Members ───────────────────────────────────────────────────────────────

    /// <summary>Get members of a department.</summary>
    [HttpGet("{departmentId:guid}/members")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<DepartmentMemberDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMembers(
        Guid departmentId,
        [FromQuery] bool activeOnly = true,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50)
        => Ok(await Mediator.Send(new GetDepartmentMembersQuery(departmentId, activeOnly, page, pageSize)));

    /// <summary>Assign a member to a department.</summary>
    [HttpPost("{departmentId:guid}/members")]
    [Authorize(Policy = AuthorizationPolicies.RequireDepartmentHead)]
    [ProducesResponseType(typeof(ApiResponse<DepartmentMemberDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> AssignMember(
        Guid departmentId,
        [FromBody] AssignMemberCommand command)
    {
        var cmd = command with { DepartmentId = departmentId };
        var result = await Mediator.Send(cmd);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    /// <summary>Remove a member from a department (soft-remove, sets LeftDate).</summary>
    [HttpDelete("members/{departmentMemberId:guid}")]
    [Authorize(Policy = AuthorizationPolicies.RequireDepartmentHead)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> RemoveMember(
        Guid departmentMemberId,
        [FromQuery] DateOnly? leftDate = null)
        => Ok(await Mediator.Send(new RemoveMemberCommand(departmentMemberId, leftDate)));

    // ── Finances ──────────────────────────────────────────────────────────────

    /// <summary>Get financial transactions for a department.</summary>
    [HttpGet("{departmentId:guid}/transactions")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<DepartmentTransactionDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTransactions(
        Guid departmentId,
        [FromQuery] DepartmentTransactionType? type = null,
        [FromQuery] DateOnly? fromDate = null,
        [FromQuery] DateOnly? toDate = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50)
        => Ok(await Mediator.Send(
            new GetDepartmentTransactionsQuery(departmentId, type, fromDate, toDate, page, pageSize)));

    /// <summary>Record an income or expense transaction for a department.</summary>
    [HttpPost("{departmentId:guid}/transactions")]
    [Authorize(Policy = AuthorizationPolicies.RequireDepartmentTreasurer)]
    [ProducesResponseType(typeof(ApiResponse<DepartmentTransactionDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> RecordTransaction(
        Guid departmentId,
        [FromBody] RecordDepartmentTransactionCommand command)
    {
        var cmd = command with { DepartmentId = departmentId };
        var result = await Mediator.Send(cmd);
        return StatusCode(StatusCodes.Status201Created, result);
    }
}
