using Application.IService;
using MediatR;
using Shared.DTOs;

namespace Application.Products.Commands;

public record UpdateProductCommand : IRequest<ProductDTO>
{
    public Guid ClientId { get; init; }
    public Guid Id { get; init; }
    public ProductDTO Product { get; init; }
}

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductDTO>
{
    private readonly IProductService _productService;

    public UpdateProductCommandHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<ProductDTO> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        return await _productService.UpdateProduct(request.ClientId, request.Id, request.Product);
    }
} 