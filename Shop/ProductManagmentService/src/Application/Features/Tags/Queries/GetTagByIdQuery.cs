using Application.IService;
using MediatR;
using Shared.DTOs;

namespace Application.Tags.Queries;

public record GetTagByIdQuery : IRequest<TagDTO>
{
    public Guid Id { get; init; }
}

public class GetTagByIdQueryHandler : IRequestHandler<GetTagByIdQuery, TagDTO>
{
    private readonly ITagService _tagService;

    public GetTagByIdQueryHandler(ITagService tagService)
    {
        _tagService = tagService;
    }

    public async Task<TagDTO> Handle(GetTagByIdQuery request, CancellationToken cancellationToken)
    {
        return await _tagService.GetById(request.Id);
    }
} 