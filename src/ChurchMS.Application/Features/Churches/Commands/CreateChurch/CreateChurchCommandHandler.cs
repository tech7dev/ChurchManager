using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.Churches.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.Churches.Commands.CreateChurch;

public class CreateChurchCommandHandler(
    IChurchRepository churchRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateChurchCommand, ApiResponse<ChurchDto>>
{
    public async Task<ApiResponse<ChurchDto>> Handle(
        CreateChurchCommand request,
        CancellationToken cancellationToken)
    {
        // Ensure church code is unique
        var existing = await churchRepository.GetByCodeAsync(request.Code, cancellationToken);
        if (existing is not null)
            throw new BadRequestException($"A church with code '{request.Code}' already exists.");

        // Validate parent church exists if specified
        if (request.ParentChurchId.HasValue)
        {
            var parent = await churchRepository.GetByIdAsync(request.ParentChurchId.Value, cancellationToken);
            if (parent is null)
                throw new NotFoundException(nameof(Church), request.ParentChurchId.Value);
        }

        var church = request.Adapt<Church>();
        await churchRepository.AddAsync(church, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = church.Adapt<ChurchDto>();
        return ApiResponse<ChurchDto>.SuccessResult(dto, "Church created successfully.");
    }
}
