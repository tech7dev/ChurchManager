using ChurchMS.Application.Features.Accounting.Commands.CreateBankAccount;
using ChurchMS.Application.Features.Accounting.Commands.RecordTransaction;
using ChurchMS.Application.Features.Accounting.DTOs;
using ChurchMS.Application.Features.Accounting.Queries.GetBankAccountList;
using ChurchMS.Application.Features.Accounting.Queries.GetTransactionList;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Constants;
using ChurchMS.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchMS.API.Controllers;

/// <summary>
/// Finance: Bank Accounts and Ledger Transactions.
/// </summary>
[Authorize]
public class AccountingController : BaseApiController
{
    // ── Bank Accounts ────────────────────────────────────────────────────────

    /// <summary>Get list of bank accounts.</summary>
    [HttpGet("bank-accounts")]
    [ProducesResponseType(typeof(ApiResponse<IList<BankAccountDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBankAccounts([FromQuery] bool activeOnly = true)
        => Ok(await Mediator.Send(new GetBankAccountListQuery(activeOnly)));

    /// <summary>Create a bank account (max 5 per church).</summary>
    [HttpPost("bank-accounts")]
    [Authorize(Policy = AuthorizationPolicies.RequireTreasurer)]
    [ProducesResponseType(typeof(ApiResponse<BankAccountDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateBankAccount([FromBody] CreateBankAccountCommand command)
    {
        var result = await Mediator.Send(command);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    // ── Transactions ─────────────────────────────────────────────────────────

    /// <summary>Get transactions for a bank account.</summary>
    [HttpGet("bank-accounts/{bankAccountId:guid}/transactions")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<AccountTransactionDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTransactions(
        Guid bankAccountId,
        [FromQuery] TransactionType? type = null,
        [FromQuery] DateOnly? fromDate = null,
        [FromQuery] DateOnly? toDate = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
        => Ok(await Mediator.Send(new GetTransactionListQuery(bankAccountId, type, fromDate, toDate, page, pageSize)));

    /// <summary>Record a manual transaction (credit or debit).</summary>
    [HttpPost("bank-accounts/{bankAccountId:guid}/transactions")]
    [Authorize(Policy = AuthorizationPolicies.RequireTreasurer)]
    [ProducesResponseType(typeof(ApiResponse<AccountTransactionDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> RecordTransaction(
        Guid bankAccountId,
        [FromBody] RecordTransactionCommand command)
    {
        // Override bankAccountId from route
        var cmd = command with { BankAccountId = bankAccountId };
        var result = await Mediator.Send(cmd);
        return StatusCode(StatusCodes.Status201Created, result);
    }
}
