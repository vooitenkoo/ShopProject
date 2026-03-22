using Application.IService;
using MediatR;

namespace Application.Products.Commands;

public record RemoveProductCommand : IRequest
{
    public Guid ClientId { get; init; }
    public Guid Id { get; init; }
}

public class RemoveProductCommandHandler : IRequestHandler<RemoveProductCommand>
{
    private readonly IProductService _productService;

    public RemoveProductCommandHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task Handle(RemoveProductCommand request, CancellationToken cancellationToken)
    {
        await _productService.RemoveProduct(request.ClientId, request.Id);
    }
} 