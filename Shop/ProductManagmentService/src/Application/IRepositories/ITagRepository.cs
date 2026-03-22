using Domain.Entities;

namespace Application.IRepository;

public interface ITagRepository {
    public Task<List<Tag>> GetAll();
    public Task<Tag> GetById(Guid id);
    public Task RemoveTag(Guid id);
    public Task<Tag> CreateTag(Tag tag);
    public Task<Tag> UpdateTag(Guid id, Tag tag);
}