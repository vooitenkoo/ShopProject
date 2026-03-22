using MediatR;
using Shared.DTOs;

namespace Application.Features.Authentication.Commands.Register;

public record RegisterCommand(RegisterUserDTO RegisterUserDto) : IRequest<bool>; 