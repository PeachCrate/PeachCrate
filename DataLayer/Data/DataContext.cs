using Microsoft.EntityFrameworkCore;
using Models.Models;

namespace DataLayer.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }
    public DbSet<User> Users { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<ProductBase> ProductBases { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Place> Places { get; set; }
    public DbSet<ShoppingList> ShoppingLists { get; set; }
    public DbSet<ShoppingProduct> ShoppingProducts { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .ToTable(tb => tb.HasTrigger("tr_AddUser"))
            .ToTable(tb => tb.HasTrigger("tr_UpdateUser"))
            .ToTable(tb => tb.HasTrigger("tr_DeleteUser"));
    }
}
