using Shared.DTOs;

namespace Application.IService;

public interface IProductService {
    public Task<List<ProductDTO>> GetAll();
    public Task<ProductDTO> GetById(Guid id);
    public Task<ProductDTO> CreateProduct(Guid clientId, ProductDTO productDTO);
    public Task RemoveProduct(Guid clientId, Guid productId);
    public Task<ProductDTO> UpdateProduct(Guid clientId, Guid productId, ProductDTO productDTO);
    public Task<TagDTO> AddTagToProduct(Guid clientId, Guid productId, Guid tagId);
    public Task<List<TagDTO>> GetAllTags(Guid productId);
    public Task RemoveTag(Guid clientId, Guid productId, Guid tagId);
    public Task<List<ProductDTO>> GetByTag(Guid tagId);
}