using ChurchMS.Application.Features.Accounting.DTOs;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Accounting.Commands.RecordTransaction;

public record RecordTransactionCommand(
    Guid BankAccountId,
    TransactionType Type,
    decimal Amount,
    string Currency,
    DateOnly TransactionDate,
    string? Description,
    string? Reference) : IRequest<ApiResponse<AccountTransactionDto>>;
