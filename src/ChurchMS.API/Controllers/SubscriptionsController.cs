using ChurchMS.Application.Features.Subscriptions.Commands.CancelSubscription;
using ChurchMS.Application.Features.Subscriptions.Commands.ChangePlan;
using ChurchMS.Application.Features.Subscriptions.Commands.CreateSubscription;
using ChurchMS.Application.Features.Subscriptions.Commands.MarkInvoicePaid;
using ChurchMS.Application.Features.Subscriptions.Commands.PurchaseSmsCredits;
using ChurchMS.Application.Features.Subscriptions.Commands.RenewSubscription;
using ChurchMS.Application.Features.Subscriptions.Queries.GetInvoiceById;
using ChurchMS.Application.Features.Subscriptions.Queries.GetInvoiceList;
using ChurchMS.Application.Features.Subscriptions.Queries.GetSmsCreditBalance;
using ChurchMS.Application.Features.Subscriptions.Queries.GetSmsCreditTransactionList;
using ChurchMS.Application.Features.Subscriptions.Queries.GetSubscription;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchMS.API.Controllers;

/// <summary>
/// Subscription plan management, invoicing, and SMS credit purchases.
/// </summary>
[Authorize]
public class SubscriptionsController : BaseApiController
{
    // ─── Subscription ─────────────────────────────────────────────────────

    /// <summary>Get the current church's active subscription.</summary>
    [HttpGet]
    public async Task<IActionResult> GetSubscription()
        => Ok(await Mediator.Send(new GetSubscriptionQuery()));

    /// <summary>Create a new subscription for the current church.</summary>
    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.RequireChurchAdmin)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateSubscription(
        [FromBody] CreateSubscriptionCommand command)
    {
        var result = await Mediator.Send(command);
        return result.Success ? StatusCode(StatusCodes.Status201Created, result) : BadRequest(result);
    }

    /// <summary>Renew the current subscription with a new payment.</summary>
    [HttpPost("renew")]
    [Authorize(Policy = AuthorizationPolicies.RequireChurchAdmin)]
    public async Task<IActionResult> RenewSubscription(
        [FromBody] RenewSubscriptionCommand command)
        => Ok(await Mediator.Send(command));

    /// <summary>Change the subscription plan (upgrade or downgrade).</summary>
    [HttpPut("{id:guid}/plan")]
    [Authorize(Policy = AuthorizationPolicies.RequireChurchAdmin)]
    public async Task<IActionResult> ChangePlan(
        Guid id, [FromBody] ChangePlanCommand command)
    {
        if (id != command.SubscriptionId)
            return BadRequest("Route ID and body ID mismatch.");
        return Ok(await Mediator.Send(command));
    }

    /// <summary>Cancel an active subscription.</summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Policy = AuthorizationPolicies.RequireChurchAdmin)]
    public async Task<IActionResult> CancelSubscription(
        Guid id, [FromBody] CancelSubscriptionCommand command)
    {
        if (id != command.SubscriptionId)
            return BadRequest("Route ID and body ID mismatch.");
        return Ok(await Mediator.Send(command));
    }

    // ─── Invoices ─────────────────────────────────────────────────────────

    /// <summary>List invoices with optional status filter.</summary>
    [HttpGet("invoices")]
    public async Task<IActionResult> GetInvoices(
        [FromQuery] InvoiceStatus? status = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
        => Ok(await Mediator.Send(new GetInvoiceListQuery(status, page, pageSize)));

    /// <summary>Get a single invoice by ID.</summary>
    [HttpGet("invoices/{id:guid}")]
    public async Task<IActionResult> GetInvoiceById(Guid id)
        => Ok(await Mediator.Send(new GetInvoiceByIdQuery(id)));

    /// <summary>Mark an invoice as paid (manual payment recording).</summary>
    [HttpPut("invoices/{id:guid}/pay")]
    [Authorize(Policy = AuthorizationPolicies.RequireTreasurer)]
    public async Task<IActionResult> MarkInvoicePaid(
        Guid id, [FromBody] MarkInvoicePaidCommand command)
    {
        if (id != command.InvoiceId)
            return BadRequest("Route ID and body ID mismatch.");
        return Ok(await Mediator.Send(command));
    }

    // ─── SMS Credits ──────────────────────────────────────────────────────

    /// <summary>Get the SMS credit balance and lifetime totals.</summary>
    [HttpGet("sms-credits")]
    public async Task<IActionResult> GetSmsCreditBalance()
        => Ok(await Mediator.Send(new GetSmsCreditBalanceQuery()));

    /// <summary>Purchase SMS credits.</summary>
    [HttpPost("sms-credits/purchase")]
    [Authorize(Policy = AuthorizationPolicies.RequireTreasurer)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> PurchaseSmsCredits(
        [FromBody] PurchaseSmsCreditsCommand command)
    {
        var result = await Mediator.Send(command);
        return result.Success ? StatusCode(StatusCodes.Status201Created, result) : BadRequest(result);
    }

    /// <summary>List SMS credit transaction history.</summary>
    [HttpGet("sms-credits/transactions")]
    public async Task<IActionResult> GetSmsCreditTransactions(
        [FromQuery] SmsCreditTransactionType? type = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
        => Ok(await Mediator.Send(new GetSmsCreditTransactionListQuery(type, page, pageSize)));
}
