using ChurchMS.Application.Features.Auth.DTOs;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenCommand : IRequest<ApiResponse<AuthResponse>>
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}
