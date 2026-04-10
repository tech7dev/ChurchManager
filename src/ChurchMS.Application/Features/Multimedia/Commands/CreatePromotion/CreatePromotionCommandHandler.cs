using ChurchMS.Application.Features.Multimedia.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Multimedia.Commands.CreatePromotion;

public class CreatePromotionCommandHandler(
    IRepository<MediaPromotion> promotionRepository,
    IRepository<MediaContent> contentRepository,
    ITenantService tenantService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreatePromotionCommand, ApiResponse<MediaPromotionDto>>
{
    public async Task<ApiResponse<MediaPromotionDto>> Handle(
        CreatePromotionCommand request, CancellationToken cancellationToken)
    {
        var churchId = tenantService.GetCurrentChurchId();
        if (!churchId.HasValue)
            return ApiResponse<MediaPromotionDto>.FailureResult("Church context required.");

        if (!request.DiscountPercent.HasValue && !request.DiscountAmount.HasValue)
            return ApiResponse<MediaPromotionDto>.FailureResult("Either DiscountPercent or DiscountAmount is required.");

        if (request.DiscountPercent.HasValue && request.DiscountAmount.HasValue)
            return ApiResponse<MediaPromotionDto>.FailureResult("Specify either DiscountPercent or DiscountAmount, not both.");

        if (request.EndDate <= request.StartDate)
            return ApiResponse<MediaPromotionDto>.FailureResult("End date must be after start date.");

        string? contentTitle = null;
        if (request.ContentId.HasValue)
        {
            var content = await contentRepository.GetByIdAsync(request.ContentId.Value, cancellationToken);
            if (content is null)
                return ApiResponse<MediaPromotionDto>.FailureResult("Content not found.");
            contentTitle = content.Title;
        }

        var promotion = new MediaPromotion
        {
            ChurchId = churchId.Value,
            ContentId = request.ContentId,
            Title = request.Title,
            Description = request.Description,
            Code = request.Code,
            DiscountPercent = request.DiscountPercent,
            DiscountAmount = request.DiscountAmount,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            IsActive = true
        };

        await promotionRepository.AddAsync(promotion, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<MediaPromotionDto>.SuccessResult(new MediaPromotionDto
        {
            Id = promotion.Id,
            ChurchId = promotion.ChurchId,
            ContentId = promotion.ContentId,
            ContentTitle = contentTitle,
            Title = promotion.Title,
            Description = promotion.Description,
            Code = promotion.Code,
            DiscountPercent = promotion.DiscountPercent,
            DiscountAmount = promotion.DiscountAmount,
            StartDate = promotion.StartDate,
            EndDate = promotion.EndDate,
            IsActive = promotion.IsActive,
            CreatedAt = promotion.CreatedAt
        });
    }
}
