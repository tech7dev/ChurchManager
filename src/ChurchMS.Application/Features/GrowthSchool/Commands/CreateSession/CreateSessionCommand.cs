using ChurchMS.Application.Features.GrowthSchool.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.GrowthSchool.Commands.CreateSession;

public record CreateSessionCommand(
    Guid CourseId,
    string Title,
    string? Description,
    string? SessionNotes,
    DateOnly SessionDate,
    int? DurationMinutes) : IRequest<ApiResponse<GrowthSchoolSessionDto>>;
