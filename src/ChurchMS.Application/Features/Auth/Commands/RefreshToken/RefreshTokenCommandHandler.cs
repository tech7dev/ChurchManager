using ChurchMS.Application.Features.Auth.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenCommandHandler(IAuthService authService)
    : IRequestHandler<RefreshTokenCommand, ApiResponse<AuthResponse>>
{
    public async Task<ApiResponse<AuthResponse>> Handle(
        RefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        var refreshRequest = new RefreshTokenRequest
        {
            AccessToken = request.AccessToken,
            RefreshToken = request.RefreshToken
        };

        return await authService.RefreshTokenAsync(refreshRequest, cancellationToken);
    }
}
