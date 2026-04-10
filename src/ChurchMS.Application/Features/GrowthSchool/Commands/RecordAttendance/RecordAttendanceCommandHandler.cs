using ChurchMS.Application.Exceptions;
using ChurchMS.Application.Features.GrowthSchool.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Domain.Entities;
using ChurchMS.Domain.Interfaces;
using ChurchMS.Shared.Models;
using Mapster;
using MediatR;

namespace ChurchMS.Application.Features.GrowthSchool.Commands.RecordAttendance;

public class RecordAttendanceCommandHandler(
    IRepository<GrowthSchoolAttendance> attendanceRepository,
    IRepository<GrowthSchoolSession> sessionRepository,
    IRepository<Member> memberRepository,
    IUnitOfWork unitOfWork,
    ITenantService tenantService)
    : IRequestHandler<RecordAttendanceCommand, ApiResponse<GrowthAttendanceDto>>
{
    public async Task<ApiResponse<GrowthAttendanceDto>> Handle(
        RecordAttendanceCommand request,
        CancellationToken cancellationToken)
    {
        var churchId = tenantService.GetCurrentChurchId()
            ?? throw new ForbiddenException("Church context is required.");

        var session = await sessionRepository.GetByIdAsync(request.SessionId, cancellationToken)
            ?? throw new NotFoundException(nameof(GrowthSchoolSession), request.SessionId);

        var member = await memberRepository.GetByIdAsync(request.MemberId, cancellationToken)
            ?? throw new NotFoundException(nameof(Member), request.MemberId);

        var attendance = new GrowthSchoolAttendance
        {
            ChurchId = churchId,
            SessionId = request.SessionId,
            MemberId = request.MemberId,
            Status = request.Status,
            Notes = request.Notes
        };

        await attendanceRepository.AddAsync(attendance, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = attendance.Adapt<GrowthAttendanceDto>();
        dto.SessionTitle = session.Title;
        dto.MemberName = $"{member.FirstName} {member.LastName}";

        return ApiResponse<GrowthAttendanceDto>.SuccessResult(dto, "Attendance recorded.");
    }
}
