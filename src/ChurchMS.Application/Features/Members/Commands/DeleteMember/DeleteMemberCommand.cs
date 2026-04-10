using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Members.Commands.DeleteMember;

public record DeleteMemberCommand(Guid Id) : IRequest<ApiResponse<bool>>;
