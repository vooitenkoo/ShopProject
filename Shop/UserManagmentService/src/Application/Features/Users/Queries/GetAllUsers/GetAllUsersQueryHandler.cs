using Application.IServices;
using MediatR;
using Shared.DTOs;

namespace Application.Features.Users.Queries.GetAllUsers;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<UserDTO>>
{
    private readonly IUserService _userService;

    public GetAllUsersQueryHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<List<UserDTO>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        return await _userService.GetAll();
    }
} 