using ChurchMS.Application.Features.Multimedia.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Multimedia.Commands.CreatePromotion;

public record CreatePromotionCommand(
    Guid? ContentId,
    string Title,
    string? Description,
    string? Code,
    decimal? DiscountPercent,
    decimal? DiscountAmount,
    DateTime StartDate,
    DateTime EndDate
) : IRequest<ApiResponse<MediaPromotionDto>>;
