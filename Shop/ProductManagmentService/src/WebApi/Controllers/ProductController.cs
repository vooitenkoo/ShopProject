using System.Security.Claims;
using Application.Products.Commands;
using Application.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Http.Logging;
using Shared.DTOs;

namespace WebApi.Controllers;

[ApiController]
[Route("api/product")]
[Authorize]
public class ProductController : ControllerBase {
    private readonly IMediator _mediator;
    
    public ProductController(IMediator mediator) =>
        _mediator = mediator;

    [HttpPost]
    [Authorize(Policy = "RequireClientRole")]
    public async Task<ProductDTO> CreateProduct([FromBody] ProductDTO productDTO) {
        try {
            var clientId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var command = new CreateProductCommand { ClientId = clientId, Product = productDTO };
            return await _mediator.Send(command);
        }
        catch(Exception ex) {
            throw new Exception("creating product was unsecesfull");
        }
    }

    [HttpGet("/bytag")]
    [AllowAnonymous]
    public async Task<List<ProductDTO>> GetByTags([FromBody] Guid tagId) {
        try {
            var query = new GetProductsByTagQuery { TagId = tagId };
            return await _mediator.Send(query);
        }
        catch(Exception ex) {
            throw new Exception("getting products was unsecesfull");
        }
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = "RequireClientRole")]
    public async Task<ProductDTO> UpdateProduct([FromBody] ProductDTO productDTO, Guid id) {
        try {
            var clientId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var command = new UpdateProductCommand { ClientId = clientId, Id = id, Product = productDTO };
            return await _mediator.Send(command);
        }
        catch(Exception ex) {
            throw new Exception("updating product was unsecesfull");
        }
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "RequireClientRole")]
    public async Task RemoveProduct(Guid id) {
        try {
            var clientId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var command = new RemoveProductCommand { ClientId = clientId, Id = id };
            await _mediator.Send(command);
        }
        catch(Exception ex) {
            throw new Exception("removing product was unsecesfull");
        }
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<List<ProductDTO>> GetAll() {
        try {
            return await _mediator.Send(new GetAllProductsQuery());
        }
        catch(Exception ex) {
            throw new Exception("getting products was unsecesfull");
        }
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<ProductDTO> GetById(Guid id) {
        try {
            return await _mediator.Send(new GetProductByIdQuery { Id = id });
        }
        catch(Exception ex) {
            throw new Exception("getting product was unsecesfull");
        }
    }

    [HttpGet("{id:guid}/tag")]
    [AllowAnonymous]
    public async Task<List<TagDTO>> GetAllTags(Guid id) {
        try {
            return await _mediator.Send(new GetProductTagsQuery { ProductId = id });
        }
        catch(Exception ex) {
            throw new Exception("getting tags was unsecesfull");
        }
    }

    [HttpPost("{id:guid}/tag")]
    [Authorize(Policy = "RequireClientRole")]
    public async Task<TagDTO> AddTagToProduct([FromBody] Guid tagId, Guid id) {
        try {
            var clientId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var command = new AddTagToProductCommand { ClientId = clientId, ProductId = id, TagId = tagId };
            return await _mediator.Send(command);
        }
        catch(Exception ex) {
            throw new Exception("adding tag to product was unsecesfull");
        }
    }

    [HttpDelete("{id:guid}/tag")]
    [Authorize(Policy = "RequireClientRole")]
    public async Task RemoveTag([FromBody] Guid tagId, Guid id) {
        try {
            var clientId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var command = new RemoveTagFromProductCommand { ClientId = clientId, ProductId = id, TagId = tagId };
            await _mediator.Send(command);
        }
        catch(Exception ex) {
            throw new Exception("removing tag from product was unsecesfull");
        }
    }
}