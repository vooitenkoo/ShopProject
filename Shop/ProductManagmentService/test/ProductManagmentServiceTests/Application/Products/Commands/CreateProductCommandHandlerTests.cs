using Application.IService;
using Application.Products.Commands;
using Moq;
using Shared.DTOs;

namespace ProductManagmentServiceTests.Application.Products.Commands;

public class CreateProductCommandHandlerTests
{
    private readonly Mock<IProductService> _productServiceMock;

    public CreateProductCommandHandlerTests()
    {
        _productServiceMock = new Mock<IProductService>();
    }

    [Fact]
    public async Task Handle_ValidCommand_CreatesAndReturnsProduct()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var command = new CreateProductCommand
        {
            ClientId = clientId,
            Product = new ProductDTO(
                null,
                "Test Product",
                99.99m,
                "Test Description"
            )
        };

        var expectedDto = new ProductDTO(
            productId,
            command.Product.ProductName,
            command.Product.ProductPrice,
            command.Product.ProductDescription
        );

        _productServiceMock.Setup(s => s.CreateProduct(clientId, command.Product))
            .ReturnsAsync(expectedDto);

        var handler = new CreateProductCommandHandler(_productServiceMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedDto.ProductId, result.ProductId);
        Assert.Equal(expectedDto.ProductName, result.ProductName);
        Assert.Equal(expectedDto.ProductPrice, result.ProductPrice);
        Assert.Equal(expectedDto.ProductDescription, result.ProductDescription);

        _productServiceMock.Verify(s => s.CreateProduct(clientId, command.Product), Times.Once);
    }
} 