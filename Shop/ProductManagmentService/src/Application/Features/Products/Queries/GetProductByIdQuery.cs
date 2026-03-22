using Application.IService;
using MediatR;
using Shared.DTOs;

namespace Application.Products.Queries;

public record GetProductByIdQuery : IRequest<ProductDTO>
{
    public Guid Id { get; init; }
}

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDTO>
{
    private readonly IProductService _productService;

    public GetProductByIdQueryHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<ProductDTO> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        return await _productService.GetById(request.Id);
    }
} 