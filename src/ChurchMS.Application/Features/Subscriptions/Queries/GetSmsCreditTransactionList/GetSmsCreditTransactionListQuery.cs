using ChurchMS.Application.Features.Subscriptions.DTOs;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Subscriptions.Queries.GetSmsCreditTransactionList;

public record GetSmsCreditTransactionListQuery(
    SmsCreditTransactionType? Type = null,
    int Page = 1,
    int PageSize = 20
) : IRequest<ApiResponse<PagedResult<SmsCreditTransactionDto>>>;
