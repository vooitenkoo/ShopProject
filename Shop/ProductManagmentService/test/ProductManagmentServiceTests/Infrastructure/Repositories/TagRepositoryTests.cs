using Domain.Entities;
using Infrastructure.DbContexts;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ProductManagmentServiceTests.Infrastructure.Repositories;

public class TagRepositoryTests : IDisposable
{
    private readonly ProductManagmentServiceDbContext _context;
    private readonly TagRepository _repository;

    public TagRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ProductManagmentServiceDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ProductManagmentServiceDbContext(options);
        _repository = new TagRepository(_context);
    }

    [Fact]
    public async Task CreateTag_ValidTag_SavesToDatabase()
    {
        // Arrange
        var tag = new Tag
        {
            TagId = Guid.NewGuid(),
            TagName = "Test Tag"
        };

        // Act
        var result = await _repository.CreateTag(tag);
        await _context.SaveChangesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(tag.TagName, result.TagName);
        
        var savedTag = await _context.Tags.FindAsync(tag.TagId);
        Assert.NotNull(savedTag);
        Assert.Equal(tag.TagName, savedTag.TagName);
    }

    [Fact]
    public async Task GetById_ExistingTag_ReturnsTag()
    {
        // Arrange
        var tag = new Tag
        {
            TagId = Guid.NewGuid(),
            TagName = "Test Tag"
        };
        _context.Tags.Add(tag);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetById(tag.TagId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(tag.TagId, result.TagId);
        Assert.Equal(tag.TagName, result.TagName);
    }

    [Fact]
    public async Task GetAll_ReturnsAllTags()
    {
        // Arrange
        var tags = new List<Tag>
        {
            new()
            {
                TagId = Guid.NewGuid(),
                TagName = "Tag 1"
            },
            new()
            {
                TagId = Guid.NewGuid(),
                TagName = "Tag 2"
            }
        };

        await _context.Tags.AddRangeAsync(tags);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAll();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains(result, t => t.TagName == "Tag 1");
        Assert.Contains(result, t => t.TagName == "Tag 2");
    }

    [Fact]
    public async Task RemoveTag_ExistingTag_RemovesFromDatabase()
    {
        // Arrange
        var tag = new Tag
        {
            TagId = Guid.NewGuid(),
            TagName = "Test Tag"
        };
        await _context.Tags.AddAsync(tag);
        await _context.SaveChangesAsync();

        // Act
        await _repository.RemoveTag(tag.TagId);
        await _context.SaveChangesAsync();

        // Assert
        var deletedTag = await _context.Tags.FindAsync(tag.TagId);
        Assert.Null(deletedTag);
    }

    [Fact]
    public async Task UpdateTag_ExistingTag_UpdatesInDatabase()
    {
        // Arrange
        var tagId = Guid.NewGuid();
        var tag = new Tag
        {
            TagId = tagId,
            TagName = "Original Tag"
        };
        await _context.Tags.AddAsync(tag);
        await _context.SaveChangesAsync();

        var updatedTag = new Tag
        {
            TagId = tagId,
            TagName = "Updated Tag"
        };

        // Act
        var result = await _repository.UpdateTag(tagId, updatedTag);
        await _context.SaveChangesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(updatedTag.TagName, result.TagName);

        var tagInDb = await _context.Tags.FindAsync(tagId);
        Assert.NotNull(tagInDb);
        Assert.Equal(updatedTag.TagName, tagInDb.TagName);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
} 