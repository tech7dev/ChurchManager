using ChurchMS.Application.Features.Members.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Members.Commands.CreateFamily;

public class CreateFamilyCommand : IRequest<ApiResponse<FamilyDto>>
{
    public string Name { get; set; } = string.Empty;
    public string? Notes { get; set; }
}
