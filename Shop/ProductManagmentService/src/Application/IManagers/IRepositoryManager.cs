using Application.IRepository;

namespace Application.IManagers;

public interface IRepositoryManager {
    IProductRepository ProductRepository {get;}
    ITagRepository TagRepository {get;}
    Task SaveAsync();
}