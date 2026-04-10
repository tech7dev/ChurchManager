using ChurchMS.Application.Features.Subscriptions.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Subscriptions.Queries.GetSmsCreditBalance;

public record GetSmsCreditBalanceQuery : IRequest<ApiResponse<SmsCreditDto>>;
