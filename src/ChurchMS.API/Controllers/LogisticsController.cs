using ChurchMS.Application.Features.Logistics.Commands.ApproveVehicleBooking;
using ChurchMS.Application.Features.Logistics.Commands.BookVehicle;
using ChurchMS.Application.Features.Logistics.Commands.CreateInventoryItem;
using ChurchMS.Application.Features.Logistics.Commands.CreateVehicle;
using ChurchMS.Application.Features.Logistics.Commands.RecordInventoryTransaction;
using ChurchMS.Application.Features.Logistics.DTOs;
using ChurchMS.Application.Features.Logistics.Queries.GetInventoryList;
using ChurchMS.Application.Features.Logistics.Queries.GetInventoryTransactions;
using ChurchMS.Application.Features.Logistics.Queries.GetVehicleBookingList;
using ChurchMS.Application.Features.Logistics.Queries.GetVehicleList;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchMS.API.Controllers;

/// <summary>
/// Logistics module: inventory management, vehicles, and bookings.
/// </summary>
[Authorize]
public class LogisticsController : BaseApiController
{
    // ── Inventory ─────────────────────────────────────────────────────────

    /// <summary>List inventory items, optionally filtered by category, status, or low stock.</summary>
    [HttpGet("inventory")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<InventoryItemDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetInventory(
        [FromQuery] string? category = null,
        [FromQuery] InventoryItemStatus? status = null,
        [FromQuery] bool lowStockOnly = false,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
        => Ok(await Mediator.Send(new GetInventoryListQuery(category, status, lowStockOnly, page, pageSize)));

    /// <summary>Create a new inventory item.</summary>
    [HttpPost("inventory")]
    [Authorize(Policy = "LogisticsManagerOrAbove")]
    [ProducesResponseType(typeof(ApiResponse<InventoryItemDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateInventoryItem([FromBody] CreateInventoryItemCommand command)
    {
        var result = await Mediator.Send(command);
        return result.Success ? StatusCode(StatusCodes.Status201Created, result) : BadRequest(result);
    }

    /// <summary>Get stock transaction history for an inventory item.</summary>
    [HttpGet("inventory/{itemId:guid}/transactions")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<InventoryTransactionDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTransactions(
        Guid itemId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
        => Ok(await Mediator.Send(new GetInventoryTransactionsQuery(itemId, page, pageSize)));

    /// <summary>Record a stock movement (check-in, check-out, adjustment) for an inventory item.</summary>
    [HttpPost("inventory/{itemId:guid}/transactions")]
    [Authorize(Policy = "LogisticsManagerOrAbove")]
    [ProducesResponseType(typeof(ApiResponse<InventoryTransactionDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> RecordTransaction(Guid itemId, [FromBody] RecordTransactionRequest request)
    {
        var command = new RecordInventoryTransactionCommand(
            itemId, request.Type, request.QuantityChange,
            request.TransactionDate, request.RelatedEventId,
            request.RecordedByMemberId, request.Notes);
        var result = await Mediator.Send(command);
        return result.Success ? StatusCode(StatusCodes.Status201Created, result) : BadRequest(result);
    }

    // ── Vehicles ──────────────────────────────────────────────────────────

    /// <summary>List vehicles, optionally filtered by status.</summary>
    [HttpGet("vehicles")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<VehicleDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetVehicles(
        [FromQuery] VehicleStatus? status = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
        => Ok(await Mediator.Send(new GetVehicleListQuery(status, page, pageSize)));

    /// <summary>Register a new church vehicle.</summary>
    [HttpPost("vehicles")]
    [Authorize(Policy = "LogisticsManagerOrAbove")]
    [ProducesResponseType(typeof(ApiResponse<VehicleDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateVehicle([FromBody] CreateVehicleCommand command)
    {
        var result = await Mediator.Send(command);
        return result.Success ? StatusCode(StatusCodes.Status201Created, result) : BadRequest(result);
    }

    // ── Vehicle Bookings ──────────────────────────────────────────────────

    /// <summary>List vehicle bookings, optionally filtered by vehicle or status.</summary>
    [HttpGet("vehicles/bookings")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<VehicleBookingDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBookings(
        [FromQuery] Guid? vehicleId = null,
        [FromQuery] VehicleBookingStatus? status = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
        => Ok(await Mediator.Send(new GetVehicleBookingListQuery(vehicleId, status, page, pageSize)));

    /// <summary>Book a vehicle.</summary>
    [HttpPost("vehicles/bookings")]
    [ProducesResponseType(typeof(ApiResponse<VehicleBookingDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> BookVehicle([FromBody] BookVehicleCommand command)
    {
        var result = await Mediator.Send(command);
        return result.Success ? StatusCode(StatusCodes.Status201Created, result) : BadRequest(result);
    }

    /// <summary>Approve a pending vehicle booking.</summary>
    [HttpPost("vehicles/bookings/{bookingId:guid}/approve")]
    [Authorize(Policy = "LogisticsManagerOrAbove")]
    [ProducesResponseType(typeof(ApiResponse<VehicleBookingDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ApproveBooking(Guid bookingId, [FromBody] ApproveBookingRequest request)
    {
        var command = new ApproveVehicleBookingCommand(bookingId, request.ApprovedByMemberId);
        return Ok(await Mediator.Send(command));
    }
}

// ── Request body models ───────────────────────────────────────────────────

public record RecordTransactionRequest(
    InventoryTransactionType Type,
    int QuantityChange,
    DateOnly TransactionDate,
    Guid? RelatedEventId,
    Guid? RecordedByMemberId,
    string? Notes);

public record ApproveBookingRequest(Guid ApprovedByMemberId);
