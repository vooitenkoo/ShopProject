using Domain.Entities;
using Xunit;

namespace ProductManagmentServiceTests.Domain;

public class ProductTests
{
    [Fact]
    public void Product_Creation_ShouldInitializeCorrectly()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var clientId = Guid.NewGuid();
        var productName = "Test Product";
        var productPrice = 99.99m;
        var productDescription = "Test Description";

        // Act
        var product = new Product
        {
            ProductId = productId,
            ProductName = productName,
            ProductPrice = productPrice,
            ProductDescription = productDescription,
            ProductCreateTime = DateTime.UtcNow,
            ClientId = clientId
        };

        // Assert
        Assert.Equal(productId, product.ProductId);
        Assert.Equal(productName, product.ProductName);
        Assert.Equal(productPrice, product.ProductPrice);
        Assert.Equal(productDescription, product.ProductDescription);
        Assert.NotNull(product.ProductCreateTime);
        Assert.Equal(clientId, product.ClientId);
        Assert.NotNull(product.Tags);
        Assert.Empty(product.Tags);
    }

    [Fact]
    public void Product_Tags_ShouldBeInitializedAsEmptyList()
    {
        // Arrange & Act
        var product = new Product();

        // Assert
        Assert.NotNull(product.Tags);
        Assert.Empty(product.Tags);
    }

    [Theory]
    [InlineData(0.01)]
    [InlineData(99.99)]
    [InlineData(1000.00)]
    public void Product_Price_ShouldAcceptValidDecimalValues(decimal price)
    {
        // Arrange
        var product = new Product();

        // Act
        product.ProductPrice = price;

        // Assert
        Assert.Equal(price, product.ProductPrice);
    }
} 