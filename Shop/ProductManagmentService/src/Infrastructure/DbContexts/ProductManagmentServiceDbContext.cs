using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DbContexts;

public class ProductManagmentServiceDbContext : DbContext {

    public ProductManagmentServiceDbContext(DbContextOptions<ProductManagmentServiceDbContext> options) : base(options) {Database.EnsureCreated();}
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Tag> Tags => Set<Tag>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .HasMany(m => m.Tags)
            .WithMany(m => m.Products);
    }

}