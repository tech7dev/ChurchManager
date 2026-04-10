using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.Churches.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.Churches.Queries.GetChurchById;

public class GetChurchByIdQueryHandler(IChurchRepository churchRepository)
    : IRequestHandler<GetChurchByIdQuery, ApiResponse<ChurchDto>>
{
    public async Task<ApiResponse<ChurchDto>> Handle(
        GetChurchByIdQuery request,
        CancellationToken cancellationToken)
    {
        var church = await churchRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Church), request.Id);

        var dto = church.Adapt<ChurchDto>();
        return ApiResponse<ChurchDto>.SuccessResult(dto);
    }
}
