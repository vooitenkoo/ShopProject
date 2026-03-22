using System.Security.Claims;
using Application.Products.Commands;
using Application.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shared.DTOs;
using WebApi.Controllers;

namespace ProductManagmentServiceTests.WebApi.Controllers;

public class ProductControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly ProductController _controller;
    private readonly Guid _testClientId = Guid.NewGuid();

    public ProductControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        
        // Setup ClaimsPrincipal
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, _testClientId.ToString())
        };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        _controller = new ProductController(_mediatorMock.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            }
        };
    }

    [Fact]
    public async Task CreateProduct_ValidProduct_ReturnsCreatedProduct()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var productDto = new ProductDTO(
            null,
            "Test Product",
            99.99m,
            "Test Description"
        );

        var expectedResult = new ProductDTO(
            productId,
            productDto.ProductName,
            productDto.ProductPrice,
            productDto.ProductDescription
        );

        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateProductCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.CreateProduct(productDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedResult.ProductId, result.ProductId);
        Assert.Equal(expectedResult.ProductName, result.ProductName);
        Assert.Equal(expectedResult.ProductPrice, result.ProductPrice);
        _mediatorMock.Verify(m => m.Send(It.Is<CreateProductCommand>(cmd => 
            cmd.ClientId == _testClientId && 
            cmd.Product == productDto), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetById_ExistingProduct_ReturnsProduct()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var expectedProduct = new ProductDTO(
            productId,
            "Test Product",
            99.99m,
            "Test Description"
        );

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetProductByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedProduct);

        // Act
        var result = await _controller.GetById(productId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedProduct.ProductId, result.ProductId);
        Assert.Equal(expectedProduct.ProductName, result.ProductName);
        _mediatorMock.Verify(m => m.Send(It.Is<GetProductByIdQuery>(q => 
            q.Id == productId), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAll_ReturnsAllProducts()
    {
        // Arrange
        var products = new List<ProductDTO>
        {
            new(
                Guid.NewGuid(),
                "Product 1",
                99.99m,
                "Description 1"
            ),
            new(
                Guid.NewGuid(),
                "Product 2",
                149.99m,
                "Description 2"
            )
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllProductsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(products);

        // Act
        var result = await _controller.GetAll();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains(result, p => p.ProductName == "Product 1");
        Assert.Contains(result, p => p.ProductName == "Product 2");
        _mediatorMock.Verify(m => m.Send(It.IsAny<GetAllProductsQuery>(), 
            It.IsAny<CancellationToken>()), Times.Once);
    }
} 