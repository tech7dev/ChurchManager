using ChurchMS.Application.Features.Secretariat.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Secretariat.Queries.GetMarriageList;

public record GetMarriageListQuery(
    Guid? MemberId = null,
    int? Year = null,
    int Page = 1,
    int PageSize = 20
) : IRequest<ApiResponse<PagedResult<MarriageRecordDto>>>;
