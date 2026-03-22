using MediatR;
using Shared.DTOs;

namespace Application.Features.Authentication.Commands.Login;

public record LoginCommand(LoginUserDTO LoginUserDto) : IRequest<string>; 