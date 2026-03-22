using Application.IService;
using MediatR;
using Shared.DTOs;

namespace Application.Products.Queries;

public record GetAllProductsQuery : IRequest<List<ProductDTO>>;

public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, List<ProductDTO>>
{
    private readonly IProductService _productService;

    public GetAllProductsQueryHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<List<ProductDTO>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        return await _productService.GetAll();
    }
} 