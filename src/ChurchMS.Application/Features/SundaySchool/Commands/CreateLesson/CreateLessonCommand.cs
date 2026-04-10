using ChurchMS.Application.Features.SundaySchool.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.SundaySchool.Commands.CreateLesson;

public record CreateLessonCommand(
    Guid ClassId,
    string Title,
    string? Description,
    string? LessonNotes,
    DateOnly LessonDate,
    int? DurationMinutes) : IRequest<ApiResponse<SundaySchoolLessonDto>>;
