using Application.IManagers;
using Application.IService;
using AutoMapper;
using Domain.Entities;
using Shared.DTOs;

namespace Infrastructure.Service;

public class ProductService : IProductService
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    public ProductService(IRepositoryManager repository, IMapper mapper) {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<TagDTO> AddTagToProduct(Guid clientId, Guid productId, Guid tagId)
    {
        var tag = await _repository.TagRepository.GetById(tagId);
        var product = await _repository.ProductRepository.GetById(productId);

        if (product.ClientId != clientId)
            throw new Exception("This is not your product, you cannot change it.");

        tag.Products.Add(product);
        product.Tags.Add(tag);
        var productDTO = _mapper.Map<ProductDTO>(await _repository.ProductRepository.UpdateProduct(productId, product));
        var tagDTO = _mapper.Map<TagDTO>(await _repository.TagRepository.UpdateTag(tagId, tag));
        await _repository.SaveAsync();
        return tagDTO;
    }

    public async Task<ProductDTO> CreateProduct(Guid clientId, ProductDTO productDTO)
    {
        var product = _mapper.Map<Product>(productDTO);
        product.ClientId = clientId;
        product.ProductCreateTime = DateTime.Now;
        var createdProduct = await _repository.ProductRepository.CreateProduct(product);
        await _repository.SaveAsync();
        return _mapper.Map<ProductDTO>(createdProduct);
    }

    public async Task<List<ProductDTO>> GetAll()
    {
        return _mapper.Map<List<ProductDTO>>(await _repository.ProductRepository.GetAll());
    }

    public async Task<List<TagDTO>> GetAllTags(Guid productId)
    {
        var product = await _repository.ProductRepository.GetById(productId);
        var tags = product.Tags;
        return _mapper.Map<List<TagDTO>>(tags); 
    }

    public async Task<ProductDTO> GetById(Guid id)
    {
        var product = await _repository.ProductRepository.GetById(id);
        return _mapper.Map<ProductDTO>(product);
    }

    public async Task<List<ProductDTO>> GetByTag(Guid tagId)
    {
        var products = await _repository.ProductRepository.GetByTag(await _repository.TagRepository.GetById(tagId));
        return _mapper.Map<List<ProductDTO>>(products);   
    }

    public async Task RemoveProduct(Guid clientId, Guid productId)
    {
        var product = await _repository.ProductRepository.GetById(productId);
        if (product.ClientId != clientId)
            throw new Exception("This is not your product, you cannot change it.");
        await _repository.ProductRepository.RemoveProduct(productId);
        await _repository.SaveAsync();
    }

    public async Task RemoveTag(Guid clientId, Guid productId, Guid tagId)
    {   
        var tag = await _repository.TagRepository.GetById(tagId);
        var product = await _repository.ProductRepository.GetById(productId);
        if (product.ClientId != clientId)
            throw new Exception("This is not your product, you cannot change it.");
        tag.Products.Remove(product);
        product.Tags.Remove(tag);
        await _repository.SaveAsync();   
    }

    public async Task<ProductDTO> UpdateProduct(Guid clientId, Guid productId, ProductDTO productDTO)
    {
        var product = await _repository.ProductRepository.GetById(productId);
        if (product.ClientId != clientId)
            throw new Exception("This is not your product, you cannot change it.");
        product.ProductDescription = productDTO.ProductDescription;
        product.ProductName = productDTO.ProductName;
        product.ProductPrice = productDTO.ProductPrice;
        var updatedProduct = await _repository.ProductRepository.UpdateProduct(productId, product);
        await _repository.SaveAsync();
        return _mapper.Map<ProductDTO>(updatedProduct);
    }
}