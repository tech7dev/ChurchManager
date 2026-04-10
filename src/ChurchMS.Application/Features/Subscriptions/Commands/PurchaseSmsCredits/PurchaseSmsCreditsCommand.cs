using ChurchMS.Application.Features.Subscriptions.DTOs;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Subscriptions.Commands.PurchaseSmsCredits;

public record PurchaseSmsCreditsCommand(
    int Credits,
    decimal Amount,
    string Currency,
    PaymentMethod PaymentMethod,
    string? PaymentReference
) : IRequest<ApiResponse<SmsCreditDto>>;
