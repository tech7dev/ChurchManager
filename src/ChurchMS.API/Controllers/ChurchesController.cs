using ChurchMS.Application.Features.Churches.Commands.CreateChurch;
using ChurchMS.Application.Features.Churches.DTOs;
using ChurchMS.Application.Features.Churches.Queries.GetChurchById;
using ChurchMS.Shared.Constants;
using ChurchMS.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchMS.API.Controllers;

[Authorize]
public class ChurchesController : BaseApiController
{
    /// <summary>
    /// Create a new church (SuperAdmin or CentralAdmin only).
    /// </summary>
    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.RequireSuperAdmin)]
    [ProducesResponseType(typeof(ApiResponse<ChurchDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<ChurchDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateChurchCommand command)
    {
        var result = await Mediator.Send(command);
        return result.Success
            ? StatusCode(StatusCodes.Status201Created, result)
            : BadRequest(result);
    }

    /// <summary>
    /// Get a church by its ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<ChurchDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<ChurchDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await Mediator.Send(new GetChurchByIdQuery(id));
        return result.Success ? Ok(result) : NotFound(result);
    }
}
