using MediatR;
using Shared.DTOs;

namespace Application.Features.Users.Queries.GetAllUsers;

public record GetAllUsersQuery() : IRequest<List<UserDTO>>; 