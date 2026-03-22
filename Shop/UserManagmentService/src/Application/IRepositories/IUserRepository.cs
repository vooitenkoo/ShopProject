using Domain.Entities;
using Shared.DTOs;

namespace Application.IRepositories;

public interface IUserRepository {

    public Task<List<User>> GetAll();
    public Task<User> GetById(Guid id);
    public Task<User> GetByName(string userName);
    public Task RemoveUser(Guid id);
    public Task<User> CreateUser(User userCreate);
    public Task<User> UpdateUser(Guid id, User userUpdate);
    public Task<bool> AnyAsync(string userName, string userEmail);
    public Task SaveChanges();
}