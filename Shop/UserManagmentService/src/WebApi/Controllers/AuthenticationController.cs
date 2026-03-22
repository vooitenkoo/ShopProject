using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using MediatR;
using Application.Features.Authentication.Commands.Login;
using Application.Features.Authentication.Commands.Register;

namespace WebApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthenticationController : ControllerBase {

    private readonly IMediator _mediator;
    public AuthenticationController(IMediator mediator) =>
        _mediator = mediator;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserDTO loginUser) {
        var token = await _mediator.Send(new LoginCommand(loginUser));
        if (string.IsNullOrEmpty(token)) 
            return Unauthorized();

        return Ok(new { Token = token });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDTO registerUser) {
        var success = await _mediator.Send(new RegisterCommand(registerUser));

        if (!success)
            return BadRequest("Username or email were already used, or password was not confirmed");

        return Ok("Registration was successful");
    }
}