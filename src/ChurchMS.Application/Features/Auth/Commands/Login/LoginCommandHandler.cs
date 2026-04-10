using ChurchMS.Application.Features.Auth.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler(IAuthService authService)
    : IRequestHandler<LoginCommand, ApiResponse<AuthResponse>>
{
    public async Task<ApiResponse<AuthResponse>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var loginRequest = new LoginRequest
        {
            Email = request.Email,
            Password = request.Password
        };

        return await authService.LoginAsync(loginRequest, cancellationToken);
    }
}
