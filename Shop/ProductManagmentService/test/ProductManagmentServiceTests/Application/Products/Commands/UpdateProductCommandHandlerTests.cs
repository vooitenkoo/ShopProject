using Application.IService;
using Application.Products.Commands;
using Moq;
using Shared.DTOs;

namespace ProductManagmentServiceTests.Application.Products.Commands;

public class UpdateProductCommandHandlerTests
{
    private readonly Mock<IProductService> _productServiceMock;

    public UpdateProductCommandHandlerTests()
    {
        _productServiceMock = new Mock<IProductService>();
    }

    [Fact]
    public async Task Handle_ValidUpdate_UpdatesAndReturnsProduct()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var clientId = Guid.NewGuid();
        var command = new UpdateProductCommand
        {
            Id = productId,
            ClientId = clientId,
            Product = new ProductDTO(
                null,
                "Updated Product",
                149.99m,
                "Updated Description"
            )
        };

        var expectedDto = new ProductDTO(
            productId,
            command.Product.ProductName,
            command.Product.ProductPrice,
            command.Product.ProductDescription
        );

        _productServiceMock.Setup(s => s.UpdateProduct(clientId, productId, command.Product))
            .ReturnsAsync(expectedDto);

        var handler = new UpdateProductCommandHandler(_productServiceMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedDto.ProductId, result.ProductId);
        Assert.Equal(expectedDto.ProductName, result.ProductName);
        Assert.Equal(expectedDto.ProductPrice, result.ProductPrice);
        Assert.Equal(expectedDto.ProductDescription, result.ProductDescription);

        _productServiceMock.Verify(s => s.UpdateProduct(clientId, productId, command.Product), Times.Once);
    }

    [Fact]
    public async Task Handle_UnauthorizedUpdate_ThrowsException()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var clientId = Guid.NewGuid();
        var command = new UpdateProductCommand
        {
            Id = productId,
            ClientId = clientId,
            Product = new ProductDTO(
                null,
                "Updated Product",
                149.99m,
                "Updated Description"
            )
        };

        _productServiceMock.Setup(s => s.UpdateProduct(clientId, productId, command.Product))
            .ThrowsAsync(new Exception("This is not your product, you cannot change it."));

        var handler = new UpdateProductCommandHandler(_productServiceMock.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => 
            handler.Handle(command, CancellationToken.None));
        Assert.Equal("This is not your product, you cannot change it.", exception.Message);
    }
}