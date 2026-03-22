using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastrucure.DbContexts;

public class UserManagmentDbContext : DbContext {

    public UserManagmentDbContext(DbContextOptions<UserManagmentDbContext> options) : base(options){Database.EnsureCreated();}
    public DbSet<User> Users => Set<User>();

}