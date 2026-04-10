using ChurchMS.Application.Features.SundaySchool.DTOs;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.SundaySchool.Commands.CreateClass;

public record CreateClassCommand(
    string Name,
    string? Description,
    ClassLevel Level,
    Guid? TeacherId,
    int? MinAge,
    int? MaxAge,
    int? MaxCapacity) : IRequest<ApiResponse<SundaySchoolClassDto>>;
