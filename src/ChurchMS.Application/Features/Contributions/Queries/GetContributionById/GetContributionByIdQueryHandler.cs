using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.Contributions.DTOs;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.Contributions.Queries.GetContributionById;

public class GetContributionByIdQueryHandler(
    IRepository<Contribution> contributionRepository)
    : IRequestHandler<GetContributionByIdQuery, ApiResponse<ContributionDto>>
{
    public async Task<ApiResponse<ContributionDto>> Handle(
        GetContributionByIdQuery request,
        CancellationToken cancellationToken)
    {
        var contribution = await contributionRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Contribution), request.Id);

        return ApiResponse<ContributionDto>.SuccessResult(contribution.Adapt<ContributionDto>());
    }
}
