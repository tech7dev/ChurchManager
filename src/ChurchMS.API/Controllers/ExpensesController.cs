using ChurchMS.Application.Features.Expenses.Commands.ApproveExpense;
using ChurchMS.Application.Features.Expenses.Commands.CreateBudget;
using ChurchMS.Application.Features.Expenses.Commands.CreateExpense;
using ChurchMS.Application.Features.Expenses.Commands.CreateExpenseCategory;
using ChurchMS.Application.Features.Expenses.Commands.MarkExpensePaid;
using ChurchMS.Application.Features.Expenses.Commands.RejectExpense;
using ChurchMS.Application.Features.Expenses.Commands.SubmitExpense;
using ChurchMS.Application.Features.Expenses.DTOs;
using ChurchMS.Application.Features.Expenses.Queries.GetBudgetList;
using ChurchMS.Application.Features.Expenses.Queries.GetExpenseById;
using ChurchMS.Application.Features.Expenses.Queries.GetExpenseCategoryList;
using ChurchMS.Application.Features.Expenses.Queries.GetExpenseList;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Constants;
using ChurchMS.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchMS.API.Controllers;

/// <summary>
/// Finance: Expenses (with approval workflow) and Budgets.
/// </summary>
[Authorize]
public class ExpensesController : BaseApiController
{
    // ── Expense Categories ────────────────────────────────────────────────────

    /// <summary>Get all expense categories.</summary>
    [HttpGet("categories")]
    [ProducesResponseType(typeof(ApiResponse<IList<ExpenseCategoryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCategories()
        => Ok(await Mediator.Send(new GetExpenseCategoryListQuery()));

    /// <summary>Create an expense category.</summary>
    [HttpPost("categories")]
    [Authorize(Policy = AuthorizationPolicies.RequireTreasurer)]
    [ProducesResponseType(typeof(ApiResponse<ExpenseCategoryDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateCategory([FromBody] CreateExpenseCategoryCommand command)
    {
        var result = await Mediator.Send(command);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    // ── Expenses ──────────────────────────────────────────────────────────────

    /// <summary>Get paginated list of expenses.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<ExpenseListDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetList(
        [FromQuery] ExpenseStatus? status = null,
        [FromQuery] Guid? categoryId = null,
        [FromQuery] DateOnly? fromDate = null,
        [FromQuery] DateOnly? toDate = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
        => Ok(await Mediator.Send(new GetExpenseListQuery(status, categoryId, fromDate, toDate, page, pageSize)));

    /// <summary>Get an expense by ID.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<ExpenseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
        => Ok(await Mediator.Send(new GetExpenseByIdQuery(id)));

    /// <summary>Create a new expense (draft state).</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<ExpenseDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateExpenseCommand command)
    {
        var result = await Mediator.Send(command);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    /// <summary>Submit an expense for approval.</summary>
    [HttpPost("{id:guid}/submit")]
    [ProducesResponseType(typeof(ApiResponse<ExpenseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Submit(Guid id)
        => Ok(await Mediator.Send(new SubmitExpenseCommand(id)));

    /// <summary>Approve a submitted expense.</summary>
    [HttpPost("{id:guid}/approve")]
    [Authorize(Policy = AuthorizationPolicies.RequireTreasurer)]
    [ProducesResponseType(typeof(ApiResponse<ExpenseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Approve(Guid id)
        => Ok(await Mediator.Send(new ApproveExpenseCommand(id)));

    /// <summary>Reject a submitted expense.</summary>
    [HttpPost("{id:guid}/reject")]
    [Authorize(Policy = AuthorizationPolicies.RequireTreasurer)]
    [ProducesResponseType(typeof(ApiResponse<ExpenseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Reject(Guid id, [FromBody] RejectExpenseRequest body)
        => Ok(await Mediator.Send(new RejectExpenseCommand(id, body.Reason)));

    /// <summary>Mark an approved expense as paid.</summary>
    [HttpPost("{id:guid}/mark-paid")]
    [Authorize(Policy = AuthorizationPolicies.RequireTreasurer)]
    [ProducesResponseType(typeof(ApiResponse<ExpenseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> MarkPaid(Guid id, [FromBody] MarkExpensePaidRequest body)
        => Ok(await Mediator.Send(new MarkExpensePaidCommand(id, body.BankAccountId, body.ReceiptUrl)));

    // ── Budgets ───────────────────────────────────────────────────────────────

    /// <summary>Get budgets, optionally filtered by year or status.</summary>
    [HttpGet("budgets")]
    [ProducesResponseType(typeof(ApiResponse<IList<BudgetDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBudgets(
        [FromQuery] int? year = null,
        [FromQuery] BudgetStatus? status = null)
        => Ok(await Mediator.Send(new GetBudgetListQuery(year, status)));

    /// <summary>Create a new budget.</summary>
    [HttpPost("budgets")]
    [Authorize(Policy = AuthorizationPolicies.RequireTreasurer)]
    [ProducesResponseType(typeof(ApiResponse<BudgetDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateBudget([FromBody] CreateBudgetCommand command)
    {
        var result = await Mediator.Send(command);
        return StatusCode(StatusCodes.Status201Created, result);
    }
}

// ── Request DTOs ─────────────────────────────────────────────────────────────
public record RejectExpenseRequest(string Reason);
public record MarkExpensePaidRequest(Guid? BankAccountId, string? ReceiptUrl);
