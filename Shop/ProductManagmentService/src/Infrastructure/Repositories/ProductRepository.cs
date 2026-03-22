using Application.IRepository;
using Domain.Entities;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ProductManagmentServiceDbContext _context;
    public ProductRepository(ProductManagmentServiceDbContext context) =>
        _context = context;
    public async Task<Product> CreateProduct(Product product)
    {
        await _context.Products
            .AddAsync(product);
        return product;
    }

    public async Task<List<Product>> GetAll()
    {
        return await _context.Products
            .Include(m => m.Tags)
            .ToListAsync();
    }

    public async Task<Product> GetById(Guid id)
    {
        return await _context.Products
            .FirstOrDefaultAsync(m => m.ProductId == id);
    }

    public async Task<List<Product>> GetByTag(Tag tag)
    {
        return await (from product in _context.Products
                        where product.Tags.Contains(tag)
                        select product).ToListAsync();
    }

    public async Task RemoveProduct(Guid id)
    {
        _context.Products
            .Remove(

                await GetById(id)

            );
    }

    public async Task<Product> UpdateProduct(Guid id, Product product)
    {
        var tempEntry = await GetById(id);
        _context.Products
            .Entry(tempEntry)
            .CurrentValues
            .SetValues(product);
        return product;
    }
}