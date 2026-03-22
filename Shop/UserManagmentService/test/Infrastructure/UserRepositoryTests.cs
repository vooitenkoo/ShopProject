using Domain.Entities;
using FluentAssertions;
using Infrastrucure.DbContexts;
using Infrastrucure.Repositories;
using Microsoft.EntityFrameworkCore;
using Shared.Enums;
using Xunit;

namespace UserManagementService.Tests.Infrastructure;

public class UserRepositoryTests
{
    private readonly DbContextOptions<UserManagmentDbContext> _options;
    private readonly UserManagmentDbContext _context;
    private readonly UserRepository _repository;

    public UserRepositoryTests()
    {
        _options = new DbContextOptionsBuilder<UserManagmentDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new UserManagmentDbContext(_options);
        _repository = new UserRepository(_context);
    }

    [Fact]
    public async Task CreateUser_Should_Add_User_To_Database()
    {
        // Arrange
        var user = new User
        {
            UserId = Guid.NewGuid(),
            UserName = "testuser",
            UserEmail = "test@example.com",
            PasswordHash = "hashedpassword",
            UserRole = Role.User,
            CreatedAt = DateTime.UtcNow,
            LastLogin = DateTime.UtcNow
        };

        // Act
        var result = await _repository.CreateUser(user);
        await _repository.SaveChanges();

        // Assert
        var savedUser = await _context.Users.FindAsync(user.UserId);
        savedUser.Should().NotBeNull();
        savedUser!.UserName.Should().Be(user.UserName);
        savedUser.UserEmail.Should().Be(user.UserEmail);
    }

    [Fact]
    public async Task GetUserById_Should_Return_Correct_User()
    {
        // Arrange
        var user = new User
        {
            UserId = Guid.NewGuid(),
            UserName = "testuser",
            UserEmail = "test@example.com",
            PasswordHash = "hashedpassword",
            UserRole = Role.User,
            CreatedAt = DateTime.UtcNow,
            LastLogin = DateTime.UtcNow
        };
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetById(user.UserId);

        // Assert
        result.Should().NotBeNull();
        result!.UserId.Should().Be(user.UserId);
        result.UserName.Should().Be(user.UserName);
    }

    [Fact]
    public async Task GetUserByEmail_Should_Return_Correct_User()
    {
        // Arrange
        var user = new User
        {
            UserId = Guid.NewGuid(),
            UserName = "testuser",
            UserEmail = "test@example.com",
            PasswordHash = "hashedpassword",
            UserRole = Role.User,
            CreatedAt = DateTime.UtcNow,
            LastLogin = DateTime.UtcNow
        };
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByName(user.UserName!);

        // Assert
        result.Should().NotBeNull();
        result!.UserEmail.Should().Be(user.UserEmail);
    }
} 