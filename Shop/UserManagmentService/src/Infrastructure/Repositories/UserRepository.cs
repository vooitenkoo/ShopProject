using Domain.Entities;
using Application.IRepositories;
using Infrastrucure.DbContexts;
using Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Infrastrucure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManagmentDbContext _context;
    public UserRepository(UserManagmentDbContext context) => _context = context;
    
    public async Task<User> CreateUser(User userCreate)
    {
        await _context.Users.AddAsync(userCreate);
        return await _context.Users.FirstOrDefaultAsync(m => m.UserEmail == userCreate.UserEmail) ?? userCreate;
    }

    public async Task<List<User>> GetAll()
    {
        return await _context.Users
            .ToListAsync();
    }

    public async Task<User> GetById(Guid id)
    {
        return await _context.Users
            .FirstOrDefaultAsync(m => m.UserId == id);
    }

    public async Task RemoveUser(Guid id)
    {
        _context.Users
            .Remove(

                await _context.Users.FirstOrDefaultAsync(m => m.UserId == id)

            );
    }

    public async Task<User> UpdateUser(Guid id, User userUpdate)
    {
        var tempEntry = await _context.Users.FirstOrDefaultAsync(m => m.UserId == id);
        
        userUpdate.UserId = id;
        _context.Users
            .Entry(tempEntry)
            .CurrentValues
            .SetValues(userUpdate);

        return userUpdate;
    }
    public async Task SaveChanges() {
        await _context.SaveChangesAsync();
    }

    public async Task<bool> AnyAsync(string userName, string userEmail)
    {
        return await _context.Users.AnyAsync(m => m.UserName == userName || m.UserEmail == userEmail);
    }

    public async Task<User> GetByName(string userName)
    {
        return await _context.Users
            .FirstOrDefaultAsync(m => m.UserName == userName);
    }
}