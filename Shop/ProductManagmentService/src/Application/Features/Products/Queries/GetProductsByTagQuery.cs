using Application.IService;
using MediatR;
using Shared.DTOs;

namespace Application.Products.Queries;

public record GetProductsByTagQuery : IRequest<List<ProductDTO>>
{
    public Guid TagId { get; init; }
}

public class GetProductsByTagQueryHandler : IRequestHandler<GetProductsByTagQuery, List<ProductDTO>>
{
    private readonly IProductService _productService;

    public GetProductsByTagQueryHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<List<ProductDTO>> Handle(GetProductsByTagQuery request, CancellationToken cancellationToken)
    {
        return await _productService.GetByTag(request.TagId);
    }
} 