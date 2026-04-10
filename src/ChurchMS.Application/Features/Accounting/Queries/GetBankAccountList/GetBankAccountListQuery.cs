using ChurchMS.Application.Features.Accounting.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Accounting.Queries.GetBankAccountList;

public record GetBankAccountListQuery(bool ActiveOnly = true) : IRequest<ApiResponse<IList<BankAccountDto>>>;
