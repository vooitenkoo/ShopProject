using Shared.DTOs;

namespace Application.IService;

public interface ITagService {
    public Task<List<TagDTO>> GetAll();
    public Task<TagDTO> GetById(Guid id);
    public Task RemoveTag(Guid id);
    public Task<TagDTO> CreateTag(TagDTO tagDTO);
    public Task<TagDTO> UpdateTag(Guid id, TagDTO tagDTO);
}