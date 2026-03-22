using Application.IService;
using MediatR;

namespace Application.Tags.Commands;

public record DeleteTagCommand : IRequest
{
    public Guid Id { get; init; }
}

public class DeleteTagCommandHandler : IRequestHandler<DeleteTagCommand>
{
    private readonly ITagService _tagService;

    public DeleteTagCommandHandler(ITagService tagService)
    {
        _tagService = tagService;
    }

    public async Task Handle(DeleteTagCommand request, CancellationToken cancellationToken)
    {
        await _tagService.RemoveTag(request.Id);
    }
} 