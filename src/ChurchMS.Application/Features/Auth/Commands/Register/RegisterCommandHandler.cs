using ChurchMS.Application.Features.Auth.DTOs;
using ChurchMS.Application.Interfaces;
using ChurchMS.Shared.Models;
using MediatR;

namespace ChurchMS.Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler(IAuthService authService)
    : IRequestHandler<RegisterCommand, ApiResponse<AuthResponse>>
{
    public async Task<ApiResponse<AuthResponse>> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        var registerRequest = new RegisterRequest
        {
            Email = request.Email,
            Password = request.Password,
            ConfirmPassword = request.ConfirmPassword,
            FirstName = request.FirstName,
            LastName = request.LastName,
            ChurchId = request.ChurchId
        };

        return await authService.RegisterAsync(registerRequest, cancellationToken);
    }
}
