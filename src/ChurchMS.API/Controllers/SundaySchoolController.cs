using ChurchMS.Application.Features.SundaySchool.Commands.CreateClass;
using ChurchMS.Application.Features.SundaySchool.Commands.CreateLesson;
using ChurchMS.Application.Features.SundaySchool.Commands.EnrollMember;
using ChurchMS.Application.Features.SundaySchool.Commands.RecordClassAttendance;
using ChurchMS.Application.Features.SundaySchool.DTOs;
using ChurchMS.Application.Features.SundaySchool.Queries.GetClassList;
using ChurchMS.Application.Features.SundaySchool.Queries.GetLessonList;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Constants;
using ChurchMS.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchMS.API.Controllers;

/// <summary>
/// Sunday School: Classes, Lessons, Enrollments, and Attendance.
/// </summary>
[Authorize]
public class SundaySchoolController : BaseApiController
{
    // ── Classes ───────────────────────────────────────────────────────────────

    /// <summary>Get list of Sunday school classes.</summary>
    [HttpGet("classes")]
    [ProducesResponseType(typeof(ApiResponse<IList<SundaySchoolClassDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetClasses(
        [FromQuery] bool activeOnly = true,
        [FromQuery] ClassLevel? level = null)
        => Ok(await Mediator.Send(new GetClassListQuery(activeOnly, level)));

    /// <summary>Create a new Sunday school class.</summary>
    [HttpPost("classes")]
    [Authorize(Policy = AuthorizationPolicies.RequireTeacher)]
    [ProducesResponseType(typeof(ApiResponse<SundaySchoolClassDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateClass([FromBody] CreateClassCommand command)
    {
        var result = await Mediator.Send(command);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    // ── Lessons ───────────────────────────────────────────────────────────────

    /// <summary>Get lessons for a class.</summary>
    [HttpGet("classes/{classId:guid}/lessons")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<SundaySchoolLessonDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLessons(
        Guid classId,
        [FromQuery] DateOnly? fromDate = null,
        [FromQuery] DateOnly? toDate = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
        => Ok(await Mediator.Send(new GetLessonListQuery(classId, fromDate, toDate, page, pageSize)));

    /// <summary>Create a lesson for a class.</summary>
    [HttpPost("classes/{classId:guid}/lessons")]
    [Authorize(Policy = AuthorizationPolicies.RequireTeacher)]
    [ProducesResponseType(typeof(ApiResponse<SundaySchoolLessonDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateLesson(Guid classId, [FromBody] CreateLessonCommand command)
    {
        var cmd = command with { ClassId = classId };
        var result = await Mediator.Send(cmd);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    // ── Enrollments ───────────────────────────────────────────────────────────

    /// <summary>Enroll a member in a class.</summary>
    [HttpPost("classes/{classId:guid}/enrollments")]
    [Authorize(Policy = AuthorizationPolicies.RequireTeacher)]
    [ProducesResponseType(typeof(ApiResponse<EnrollmentDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> EnrollMember(Guid classId, [FromBody] EnrollMemberCommand command)
    {
        var cmd = command with { ClassId = classId };
        var result = await Mediator.Send(cmd);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    // ── Attendance ────────────────────────────────────────────────────────────

    /// <summary>Record attendance for a lesson.</summary>
    [HttpPost("lessons/{lessonId:guid}/attendance")]
    [Authorize(Policy = AuthorizationPolicies.RequireTeacher)]
    [ProducesResponseType(typeof(ApiResponse<ClassAttendanceDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> RecordAttendance(
        Guid lessonId,
        [FromBody] RecordClassAttendanceCommand command)
    {
        var cmd = command with { LessonId = lessonId };
        var result = await Mediator.Send(cmd);
        return StatusCode(StatusCodes.Status201Created, result);
    }
}
