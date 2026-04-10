using ChurchMS.Application.Features.GrowthSchool.DTOs;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.GrowthSchool.Commands.CreateCourse;

public record CreateCourseCommand(
    string Name,
    string? Description,
    GrowthSchoolLevel Level,
    Guid? InstructorId,
    int? DurationWeeks,
    int? MaxCapacity) : IRequest<ApiResponse<GrowthSchoolCourseDto>>;
