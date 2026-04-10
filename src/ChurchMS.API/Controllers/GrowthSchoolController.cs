using ChurchMS.Application.Features.GrowthSchool.Commands.CreateCourse;
using ChurchMS.Application.Features.GrowthSchool.Commands.CreateSession;
using ChurchMS.Application.Features.GrowthSchool.Commands.EnrollMember;
using ChurchMS.Application.Features.GrowthSchool.Commands.RecordAttendance;
using ChurchMS.Application.Features.GrowthSchool.DTOs;
using ChurchMS.Application.Features.GrowthSchool.Queries.GetCourseList;
using ChurchMS.Application.Features.GrowthSchool.Queries.GetSessionList;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Constants;
using ChurchMS.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchMS.API.Controllers;

/// <summary>
/// Growth School: Discipleship courses, sessions, enrollments, and attendance.
/// </summary>
[Authorize]
public class GrowthSchoolController : BaseApiController
{
    // ── Courses ───────────────────────────────────────────────────────────────

    /// <summary>Get list of growth school courses.</summary>
    [HttpGet("courses")]
    [ProducesResponseType(typeof(ApiResponse<IList<GrowthSchoolCourseDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCourses(
        [FromQuery] bool activeOnly = true,
        [FromQuery] GrowthSchoolLevel? level = null)
        => Ok(await Mediator.Send(new GetCourseListQuery(activeOnly, level)));

    /// <summary>Create a new growth school course.</summary>
    [HttpPost("courses")]
    [Authorize(Policy = AuthorizationPolicies.RequireTeacher)]
    [ProducesResponseType(typeof(ApiResponse<GrowthSchoolCourseDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateCourse([FromBody] CreateCourseCommand command)
    {
        var result = await Mediator.Send(command);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    // ── Sessions ──────────────────────────────────────────────────────────────

    /// <summary>Get sessions for a course.</summary>
    [HttpGet("courses/{courseId:guid}/sessions")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<GrowthSchoolSessionDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSessions(
        Guid courseId,
        [FromQuery] DateOnly? fromDate = null,
        [FromQuery] DateOnly? toDate = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
        => Ok(await Mediator.Send(new GetSessionListQuery(courseId, fromDate, toDate, page, pageSize)));

    /// <summary>Create a session for a course.</summary>
    [HttpPost("courses/{courseId:guid}/sessions")]
    [Authorize(Policy = AuthorizationPolicies.RequireTeacher)]
    [ProducesResponseType(typeof(ApiResponse<GrowthSchoolSessionDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateSession(Guid courseId, [FromBody] CreateSessionCommand command)
    {
        var cmd = command with { CourseId = courseId };
        var result = await Mediator.Send(cmd);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    // ── Enrollments ───────────────────────────────────────────────────────────

    /// <summary>Enroll a member in a course.</summary>
    [HttpPost("courses/{courseId:guid}/enrollments")]
    [Authorize(Policy = AuthorizationPolicies.RequireTeacher)]
    [ProducesResponseType(typeof(ApiResponse<GrowthEnrollmentDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> EnrollMember(Guid courseId, [FromBody] EnrollMemberCommand command)
    {
        var cmd = command with { CourseId = courseId };
        var result = await Mediator.Send(cmd);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    // ── Attendance ────────────────────────────────────────────────────────────

    /// <summary>Record attendance for a session.</summary>
    [HttpPost("sessions/{sessionId:guid}/attendance")]
    [Authorize(Policy = AuthorizationPolicies.RequireTeacher)]
    [ProducesResponseType(typeof(ApiResponse<GrowthAttendanceDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> RecordAttendance(
        Guid sessionId,
        [FromBody] RecordAttendanceCommand command)
    {
        var cmd = command with { SessionId = sessionId };
        var result = await Mediator.Send(cmd);
        return StatusCode(StatusCodes.Status201Created, result);
    }
}
