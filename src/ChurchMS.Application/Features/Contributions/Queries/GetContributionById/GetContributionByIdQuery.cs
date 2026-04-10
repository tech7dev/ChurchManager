using ChurchMS.Application.Features.Contributions.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Contributions.Queries.GetContributionById;

public record GetContributionByIdQuery(Guid Id) : IRequest<ApiResponse<ContributionDto>>;
