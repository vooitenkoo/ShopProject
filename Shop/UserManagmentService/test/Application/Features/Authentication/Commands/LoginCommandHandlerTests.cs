using Application.Features.Authentication.Commands.Login;
using Application.IServices;
using FluentAssertions;
using Moq;
using Shared.DTOs;
using Xunit;

namespace UserManagementService.Tests.Application.Features.Authentication.Commands;

public class LoginCommandHandlerTests
{
    private readonly Mock<IAuthenticationService> _authServiceMock;
    private readonly LoginCommandHandler _handler;

    public LoginCommandHandlerTests()
    {
        _authServiceMock = new Mock<IAuthenticationService>();
        _handler = new LoginCommandHandler(_authServiceMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_Token_For_Valid_Credentials()
    {
        // Arrange
        var loginDto = new LoginUserDTO(
            UserName: "testuser",
            UserPassword: "password123"
        );

        var command = new LoginCommand(loginDto);
        var expectedToken = "jwt_token";

        _authServiceMock.Setup(s => s.AuthenticateUser(loginDto))
            .ReturnsAsync(expectedToken);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(expectedToken);
        _authServiceMock.Verify(s => s.AuthenticateUser(loginDto), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Return_Null_For_Invalid_Credentials()
    {
        // Arrange
        var loginDto = new LoginUserDTO(
            UserName: "testuser",
            UserPassword: "wrongpassword"
        );

        var command = new LoginCommand(loginDto);

        _authServiceMock.Setup(s => s.AuthenticateUser(loginDto))
            .ReturnsAsync((string?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeNull();
        _authServiceMock.Verify(s => s.AuthenticateUser(loginDto), Times.Once);
    }
} 