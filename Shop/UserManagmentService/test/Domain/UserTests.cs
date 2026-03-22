using Domain.Entities;
using FluentAssertions;
using Shared.Enums;
using Xunit;

namespace UserManagementService.Tests.Domain;

public class UserTests
{
    [Fact]
    public void User_Creation_Should_Set_Properties_Correctly()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var now = DateTime.UtcNow;

        // Act
        var user = new User
        {
            UserId = userId,
            UserName = "testuser",
            UserEmail = "test@example.com",
            PasswordHash = "hashedpassword",
            UserRole = Role.User,
            CreatedAt = now,
            LastLogin = now
        };

        // Assert
        user.UserId.Should().Be(userId);
        user.UserName.Should().Be("testuser");
        user.UserEmail.Should().Be("test@example.com");
        user.PasswordHash.Should().Be("hashedpassword");
        user.UserRole.Should().Be(Role.User);
        user.CreatedAt.Should().Be(now);
        user.LastLogin.Should().Be(now);
    }

    [Theory]
    [InlineData(Role.Admin)]
    [InlineData(Role.User)]
    public void User_Should_Support_Different_Roles(Role role)
    {
        // Arrange & Act
        var user = new User { UserRole = role };

        // Assert
        user.UserRole.Should().Be(role);
    }
} 