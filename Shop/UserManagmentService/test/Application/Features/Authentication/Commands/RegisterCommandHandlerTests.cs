using Application.Features.Authentication.Commands.Register;
using Application.IServices;
using FluentAssertions;
using Moq;
using Shared.DTOs;
using Shared.Enums;
using Xunit;

namespace UserManagementService.Tests.Application.Features.Authentication.Commands;

public class RegisterCommandHandlerTests
{
    private readonly Mock<IAuthenticationService> _authServiceMock;
    private readonly RegisterCommandHandler _handler;

    public RegisterCommandHandlerTests()
    {
        _authServiceMock = new Mock<IAuthenticationService>();
        _handler = new RegisterCommandHandler(_authServiceMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_True_For_Valid_Registration()
    {
        // Arrange
        var registerDto = new RegisterUserDTO(
            UserName: "testuser",
            UserEmail: "test@example.com",
            UserRole: Role.User,
            UserPassword: "Password123",
            UserVerifyPassword: "Password123"
        );

        var command = new RegisterCommand(registerDto);

        _authServiceMock.Setup(s => s.RegistrateUser(registerDto))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        _authServiceMock.Verify(s => s.RegistrateUser(registerDto), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Return_False_For_Invalid_Registration()
    {
        // Arrange
        var registerDto = new RegisterUserDTO(
            UserName: "testuser",
            UserEmail: "test@example.com",
            UserRole: Role.User,
            UserPassword: "Password123",
            UserVerifyPassword: "DifferentPassword123"
        );

        var command = new RegisterCommand(registerDto);

        _authServiceMock.Setup(s => s.RegistrateUser(registerDto))
            .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeFalse();
        _authServiceMock.Verify(s => s.RegistrateUser(registerDto), Times.Once);
    }
} 