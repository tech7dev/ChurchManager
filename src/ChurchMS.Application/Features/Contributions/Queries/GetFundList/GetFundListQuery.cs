using ChurchMS.Application.Features.Contributions.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Contributions.Queries.GetFundList;

public record GetFundListQuery(bool ActiveOnly = true) : IRequest<ApiResponse<IList<FundDto>>>;
