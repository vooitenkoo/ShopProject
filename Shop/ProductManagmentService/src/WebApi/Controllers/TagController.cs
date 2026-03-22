using Application.Tags.Commands;
using Application.Tags.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;

namespace WebApi.Controllers;

[ApiController]
[Route("api/tag")]
[Authorize]
public class TagController : ControllerBase {
    private readonly IMediator _mediator;
    
    public TagController(IMediator mediator) =>
        _mediator = mediator;

    [HttpPost]
    [Authorize(Policy = "RequireAdminRole")]
    public async Task<TagDTO> CreateTag([FromBody] TagDTO tagDTO) {
        try {
            return await _mediator.Send(new CreateTagCommand { Tag = tagDTO });
        }
        catch (Exception ex) {
            throw new Exception("creating tag was unsuccesfull");
        }
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = "RequireAdminRole")]
    public async Task<TagDTO> UpdateTag([FromBody] TagDTO tagDTO, Guid id) {
        try {
            return await _mediator.Send(new UpdateTagCommand { Id = id, Tag = tagDTO });
        }
        catch (Exception ex) {
            throw new Exception("updating tag was unsuccesfull");
        }
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "RequireAdminRole")]
    public async Task DeleteTag(Guid id) {
        try {
            await _mediator.Send(new DeleteTagCommand { Id = id });
        }
        catch (Exception ex) {
            throw new Exception("removing tag was unsuccesfull");
        }
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<List<TagDTO>> GetAll() {
        try {
            return await _mediator.Send(new GetAllTagsQuery());
        }
        catch (Exception ex) {
            throw new Exception("getting all tags was unsuccesfull");
        }
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<TagDTO> GetById(Guid id) {
        try {
            return await _mediator.Send(new GetTagByIdQuery { Id = id });
        }
        catch (Exception ex) {
            throw new Exception("getting tag was unsuccesfull");
        }
    }
}