using Application.IService;
using MediatR;
using Shared.DTOs;

namespace Application.Tags.Queries;

public record GetAllTagsQuery : IRequest<List<TagDTO>>;

public class GetAllTagsQueryHandler : IRequestHandler<GetAllTagsQuery, List<TagDTO>>
{
    private readonly ITagService _tagService;

    public GetAllTagsQueryHandler(ITagService tagService)
    {
        _tagService = tagService;
    }

    public async Task<List<TagDTO>> Handle(GetAllTagsQuery request, CancellationToken cancellationToken)
    {
        return await _tagService.GetAll();
    }
} 