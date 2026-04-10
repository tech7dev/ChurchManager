using ChurchMS.Application.Features.Accounting.DTOs;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Accounting.Commands.CreateBankAccount;

public record CreateBankAccountCommand(
    string AccountName,
    string AccountNumber,
    string BankName,
    string? BranchName,
    string? SwiftCode,
    string? RoutingNumber,
    BankAccountType AccountType,
    string Currency,
    bool IsDefault) : IRequest<ApiResponse<BankAccountDto>>;
