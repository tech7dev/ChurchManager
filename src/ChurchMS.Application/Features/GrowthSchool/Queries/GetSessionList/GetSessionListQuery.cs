using ChurchMS.Application.Features.GrowthSchool.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.GrowthSchool.Queries.GetSessionList;

public record GetSessionListQuery(
    Guid CourseId,
    DateOnly? FromDate = null,
    DateOnly? ToDate = null,
    int Page = 1,
    int PageSize = 20) : IRequest<ApiResponse<PagedResult<GrowthSchoolSessionDto>>>;
