using Application.IService;
using MediatR;
using Shared.DTOs;

namespace Application.Products.Queries;

public record GetProductTagsQuery : IRequest<List<TagDTO>>
{
    public Guid ProductId { get; init; }
}

public class GetProductTagsQueryHandler : IRequestHandler<GetProductTagsQuery, List<TagDTO>>
{
    private readonly IProductService _productService;

    public GetProductTagsQueryHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<List<TagDTO>> Handle(GetProductTagsQuery request, CancellationToken cancellationToken)
    {
        return await _productService.GetAllTags(request.ProductId);
    }
} 