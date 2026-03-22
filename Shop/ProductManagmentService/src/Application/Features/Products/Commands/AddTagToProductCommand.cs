using Application.IService;
using MediatR;
using Shared.DTOs;

namespace Application.Products.Commands;

public record AddTagToProductCommand : IRequest<TagDTO>
{
    public Guid ClientId { get; init; }
    public Guid ProductId { get; init; }
    public Guid TagId { get; init; }
}

public class AddTagToProductCommandHandler : IRequestHandler<AddTagToProductCommand, TagDTO>
{
    private readonly IProductService _productService;

    public AddTagToProductCommandHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<TagDTO> Handle(AddTagToProductCommand request, CancellationToken cancellationToken)
    {
        return await _productService.AddTagToProduct(request.ClientId, request.ProductId, request.TagId);
    }
} 