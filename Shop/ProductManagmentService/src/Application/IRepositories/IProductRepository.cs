using Domain.Entities;

namespace Application.IRepository;

public interface IProductRepository {
    public Task<List<Product>> GetAll();
    public Task<Product> GetById(Guid id);
    public Task RemoveProduct(Guid id);
    public Task<Product> UpdateProduct(Guid id, Product product);
    public Task<Product> CreateProduct(Product product);
    public Task<List<Product>> GetByTag(Tag tag);
}