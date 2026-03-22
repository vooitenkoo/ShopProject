using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Features.Users.Commands.DeleteUser;
using Application.Features.Users.Commands.UpdateUser;
using Application.Features.Users.Queries.GetAllUsers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;

namespace WebApi.Controllers;

[ApiController]
[Route("api/user")]
[Authorize]
public class UserController : ControllerBase {

    private readonly IMediator _mediator;
    public UserController(IMediator mediator) =>
        _mediator = mediator;

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "RequireAdminRole")]
    public async Task<ActionResult> DeleteUser(Guid id) {
        try {
            await _mediator.Send(new DeleteUserCommand(id));
            return Ok("User was successfully removed");
        }
        catch(Exception ex) {
            throw new Exception("deleting user was unsuccessful");
        }
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = "RequireClientRole")]
    [Authorize(Policy = "RequireAdminRole")]
    public async Task<ActionResult> UpdateUser([FromBody] UpdateUserDTO updateUser, Guid id) {
        try {
            var userDTO = await _mediator.Send(new UpdateUserCommand(id, updateUser));
            return Ok("User was successfully updated: " + userDTO);
        }
        catch(Exception ex) {
            throw new Exception("updating user was unsuccessful");
        }
    }

    [HttpGet]
    [Authorize(Policy = "RequireAdminRole")]
    public async Task<ActionResult<List<UserDTO>>> GetAll() {
        try {
            return Ok(await _mediator.Send(new GetAllUsersQuery()));
        }
        catch(Exception ex) {
            throw new Exception("getting users was unsuccessful");
        }
    }
}