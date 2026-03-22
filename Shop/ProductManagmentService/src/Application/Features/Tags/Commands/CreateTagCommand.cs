using Application.IService;
using MediatR;
using Shared.DTOs;

namespace Application.Tags.Commands;

public record CreateTagCommand : IRequest<TagDTO>
{
    public TagDTO Tag { get; init; }
}

public class CreateTagCommandHandler : IRequestHandler<CreateTagCommand, TagDTO>
{
    private readonly ITagService _tagService;

    public CreateTagCommandHandler(ITagService tagService)
    {
        _tagService = tagService;
    }

    public async Task<TagDTO> Handle(CreateTagCommand request, CancellationToken cancellationToken)
    {
        return await _tagService.CreateTag(request.Tag);
    }
} 