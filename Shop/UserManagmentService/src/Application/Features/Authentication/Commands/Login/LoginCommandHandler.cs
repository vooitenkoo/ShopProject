using Application.IServices;
using MediatR;

namespace Application.Features.Authentication.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, string>
{
    private readonly IAuthenticationService _authenticationService;

    public LoginCommandHandler(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        return await _authenticationService.AuthenticateUser(request.LoginUserDto);
    }
} 