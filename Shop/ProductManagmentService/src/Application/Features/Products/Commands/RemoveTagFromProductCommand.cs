using Application.IService;
using MediatR;

namespace Application.Products.Commands;

public record RemoveTagFromProductCommand : IRequest
{
    public Guid ClientId { get; init; }
    public Guid ProductId { get; init; }
    public Guid TagId { get; init; }
}

public class RemoveTagFromProductCommandHandler : IRequestHandler<RemoveTagFromProductCommand>
{
    private readonly IProductService _productService;

    public RemoveTagFromProductCommandHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task Handle(RemoveTagFromProductCommand request, CancellationToken cancellationToken)
    {
        await _productService.RemoveTag(request.ClientId, request.ProductId, request.TagId);
    }
} 