using Application.Features.Users.Commands.UpdateUser;
using Application.Features.Users.Commands.DeleteUser;
using Application.Features.Users.Queries.GetAllUsers;
using Domain.Entities;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shared.DTOs;
using Shared.Enums;
using WebApi.Controllers;
using Xunit;

namespace UserManagementService.Tests.WebApi;

public class UserControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly UserController _controller;

    public UserControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new UserController(_mediatorMock.Object);
    }

    [Fact]
    public async Task GetAll_Should_Return_OkResult_With_Users()
    {
        // Arrange
        var users = new List<UserDTO>
        {
            new UserDTO(
                UserName: "testuser1",
                UserEmail: "test1@example.com",
                UserRole: Role.User,
                IsActive: true,
                CreatedAt: DateTime.UtcNow
            ),
            new UserDTO(
                UserName: "testuser2",
                UserEmail: "test2@example.com",
                UserRole: Role.Admin,
                IsActive: true,
                CreatedAt: DateTime.UtcNow
            )
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllUsersQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(users);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        var returnedUsers = okResult!.Value as List<UserDTO>;
        returnedUsers.Should().NotBeNull();
        returnedUsers.Should().HaveCount(2);
        returnedUsers![0].UserName.Should().Be("testuser1");
        returnedUsers[1].UserName.Should().Be("testuser2");
    }

    [Fact]
    public async Task UpdateUser_Should_Return_OkResult()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var updateDto = new UpdateUserDTO(
            UserName: "newuser",
            UserEmail: "new@example.com"
        );

        var updatedUserDto = new UserDTO(
            UserName: updateDto.UserName,
            UserEmail: updateDto.UserEmail,
            UserRole: Role.User,
            IsActive: true,
            CreatedAt: DateTime.UtcNow
        );

        _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(updatedUserDto);

        // Act
        var result = await _controller.UpdateUser(updateDto, userId);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeOfType<string>();
        okResult.Value.As<string>().Should().Contain(updatedUserDto.UserName);
    }

    [Fact]
    public async Task DeleteUser_Should_Return_OkResult()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteUser(userId);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeOfType<string>();
        okResult.Value.As<string>().Should().Be("User was successfully removed");
    }
} 