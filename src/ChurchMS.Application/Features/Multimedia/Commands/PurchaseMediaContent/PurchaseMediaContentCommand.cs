using ChurchMS.Application.Features.Multimedia.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Multimedia.Commands.PurchaseMediaContent;

public record PurchaseMediaContentCommand(
    Guid ContentId,
    Guid MemberId,
    string? PaymentReference
) : IRequest<ApiResponse<MediaPurchaseDto>>;
