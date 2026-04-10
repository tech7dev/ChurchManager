using ChurchMS.Application.Features.SundaySchool.DTOs;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.SundaySchool.Queries.GetClassList;

public record GetClassListQuery(
    bool ActiveOnly = true,
    ClassLevel? Level = null) : IRequest<ApiResponse<IList<SundaySchoolClassDto>>>;
