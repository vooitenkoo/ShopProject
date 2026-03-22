using Application.IService;
using MediatR;
using Shared.DTOs;

namespace Application.Products.Commands;

public record CreateProductCommand : IRequest<ProductDTO>
{
    public Guid ClientId { get; init; }
    public ProductDTO Product { get; init; }
}

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDTO>
{
    private readonly IProductService _productService;

    public CreateProductCommandHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<ProductDTO> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        return await _productService.CreateProduct(request.ClientId, request.Product);
    }
} 