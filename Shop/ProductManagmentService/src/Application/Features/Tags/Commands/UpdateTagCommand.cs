using Application.IService;
using MediatR;
using Shared.DTOs;

namespace Application.Tags.Commands;

public record UpdateTagCommand : IRequest<TagDTO>
{
    public Guid Id { get; init; }
    public TagDTO Tag { get; init; }
}

public class UpdateTagCommandHandler : IRequestHandler<UpdateTagCommand, TagDTO>
{
    private readonly ITagService _tagService;

    public UpdateTagCommandHandler(ITagService tagService)
    {
        _tagService = tagService;
    }

    public async Task<TagDTO> Handle(UpdateTagCommand request, CancellationToken cancellationToken)
    {
        return await _tagService.UpdateTag(request.Id, request.Tag);
    }
} 