using ChurchMS.Application.Features.Members.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Members.Commands.ConvertVisitorToMember;

public record ConvertVisitorToMemberCommand(Guid VisitorId) : IRequest<ApiResponse<MemberDto>>;
