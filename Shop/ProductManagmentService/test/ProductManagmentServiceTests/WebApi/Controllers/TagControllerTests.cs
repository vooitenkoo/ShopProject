using Application.Tags.Commands;
using Application.Tags.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shared.DTOs;
using WebApi.Controllers;

namespace ProductManagmentServiceTests.WebApi.Controllers;

public class TagControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly TagController _controller;

    public TagControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new TagController(_mediatorMock.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };
    }

    [Fact]
    public async Task CreateTag_ValidTag_ReturnsCreatedTag()
    {
        // Arrange
        var tagId = Guid.NewGuid();
        var tagDto = new TagDTO(
            null,
            "Test Tag"
        );

        var expectedResult = new TagDTO(
            tagId,
            tagDto.TagName
        );

        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateTagCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.CreateTag(tagDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedResult.TagId, result.TagId);
        Assert.Equal(expectedResult.TagName, result.TagName);
        _mediatorMock.Verify(m => m.Send(It.Is<CreateTagCommand>(cmd => 
            cmd.Tag == tagDto), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAll_ReturnsAllTags()
    {
        // Arrange
        var tags = new List<TagDTO>
        {
            new(Guid.NewGuid(), "Tag 1"),
            new(Guid.NewGuid(), "Tag 2")
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllTagsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(tags);

        // Act
        var result = await _controller.GetAll();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains(result, t => t.TagName == "Tag 1");
        Assert.Contains(result, t => t.TagName == "Tag 2");
        _mediatorMock.Verify(m => m.Send(It.IsAny<GetAllTagsQuery>(), 
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteTag_ValidId_CallsDeleteCommand()
    {
        // Arrange
        var tagId = Guid.NewGuid();

        // Act
        await _controller.DeleteTag(tagId);

        // Assert
        _mediatorMock.Verify(m => m.Send(It.Is<DeleteTagCommand>(cmd => 
            cmd.Id == tagId), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetById_ExistingTag_ReturnsTag()
    {
        // Arrange
        var tagId = Guid.NewGuid();
        var expectedTag = new TagDTO(
            tagId,
            "Test Tag"
        );

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetTagByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedTag);

        // Act
        var result = await _controller.GetById(tagId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedTag.TagId, result.TagId);
        Assert.Equal(expectedTag.TagName, result.TagName);
        _mediatorMock.Verify(m => m.Send(It.Is<GetTagByIdQuery>(q => 
            q.Id == tagId), It.IsAny<CancellationToken>()), Times.Once);
    }
} 