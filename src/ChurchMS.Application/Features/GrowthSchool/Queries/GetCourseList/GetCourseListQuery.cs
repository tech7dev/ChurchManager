using ChurchMS.Application.Features.GrowthSchool.DTOs;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.GrowthSchool.Queries.GetCourseList;

public record GetCourseListQuery(
    bool ActiveOnly = true,
    GrowthSchoolLevel? Level = null) : IRequest<ApiResponse<IList<GrowthSchoolCourseDto>>>;
