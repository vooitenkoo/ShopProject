using Application.IManagers;
using Application.IRepository;
using Infrastructure.DbContexts;
using Infrastructure.Repositories;

namespace Infrastructure.Managers;

public class RepositoryManager : IRepositoryManager
{
    private readonly ProductManagmentServiceDbContext _context;
    private readonly Lazy<IProductRepository> _productRepository;
    private readonly Lazy<ITagRepository> _tagRepository;
    public RepositoryManager(ProductManagmentServiceDbContext context) {

        _context = context;
        _productRepository = new Lazy<IProductRepository>(new ProductRepository(context));
        _tagRepository = new Lazy<ITagRepository>(new TagRepository(context));

    }
    public IProductRepository ProductRepository => _productRepository.Value;

    public ITagRepository TagRepository => _tagRepository.Value;

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}