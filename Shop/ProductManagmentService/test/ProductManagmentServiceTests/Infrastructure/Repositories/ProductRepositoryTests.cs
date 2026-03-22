using Domain.Entities;
using Infrastructure.DbContexts;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ProductManagmentServiceTests.Infrastructure.Repositories;

public class ProductRepositoryTests : IDisposable
{
    private readonly ProductManagmentServiceDbContext _context;
    private readonly ProductRepository _repository;

    public ProductRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ProductManagmentServiceDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ProductManagmentServiceDbContext(options);
        _repository = new ProductRepository(_context);
    }

    [Fact]
    public async Task CreateProduct_ValidProduct_SavesToDatabase()
    {
        // Arrange
        var product = new Product
        {
            ProductId = Guid.NewGuid(),
            ClientId = Guid.NewGuid(),
            ProductName = "Test Product",
            ProductPrice = 99.99m,
            ProductDescription = "Test Description",
            ProductCreateTime = DateTime.UtcNow
        };

        // Act
        var result = await _repository.CreateProduct(product);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(product.ProductId, result.ProductId);
        
        var savedProduct = await _context.Products.FindAsync(product.ProductId);
        Assert.NotNull(savedProduct);
        Assert.Equal(product.ProductName, savedProduct.ProductName);
        Assert.Equal(product.ProductPrice, savedProduct.ProductPrice);
    }

    [Fact]
    public async Task GetById_ExistingProduct_ReturnsProduct()
    {
        // Arrange
        var product = new Product
        {
            ProductId = Guid.NewGuid(),
            ClientId = Guid.NewGuid(),
            ProductName = "Test Product",
            ProductPrice = 99.99m,
            ProductDescription = "Test Description",
            ProductCreateTime = DateTime.UtcNow
        };
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetById(product.ProductId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(product.ProductId, result.ProductId);
        Assert.Equal(product.ProductName, result.ProductName);
    }

    [Fact]
    public async Task GetAll_ReturnsAllProducts()
    {
        // Arrange
        var products = new List<Product>
        {
            new()
            {
                ProductId = Guid.NewGuid(),
                ClientId = Guid.NewGuid(),
                ProductName = "Product 1",
                ProductPrice = 99.99m,
                ProductDescription = "Description 1"
            },
            new()
            {
                ProductId = Guid.NewGuid(),
                ClientId = Guid.NewGuid(),
                ProductName = "Product 2",
                ProductPrice = 149.99m,
                ProductDescription = "Description 2"
            }
        };

        await _context.Products.AddRangeAsync(products);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAll();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains(result, p => p.ProductName == "Product 1" && p.ProductPrice == 99.99m);
        Assert.Contains(result, p => p.ProductName == "Product 2" && p.ProductPrice == 149.99m);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
} 