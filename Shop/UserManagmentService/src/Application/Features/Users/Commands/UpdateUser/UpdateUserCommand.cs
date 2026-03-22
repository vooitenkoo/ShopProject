using MediatR;
using Shared.DTOs;

namespace Application.Features.Users.Commands.UpdateUser;

public record UpdateUserCommand(Guid Id, UpdateUserDTO UpdateUserDto) : IRequest<UserDTO>; 