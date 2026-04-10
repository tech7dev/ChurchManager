using ChurchMS.Application.Features.SundaySchool.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.SundaySchool.Queries.GetLessonList;

public record GetLessonListQuery(
    Guid ClassId,
    DateOnly? FromDate = null,
    DateOnly? ToDate = null,
    int Page = 1,
    int PageSize = 20) : IRequest<ApiResponse<PagedResult<SundaySchoolLessonDto>>>;
