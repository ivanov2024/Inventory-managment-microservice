using InventoryManagment.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagment.Data;

public class ApplicationDbContext : DbContext
{
    const string _connectionString = "Server=.;Database=InventoryManagmentDb;TrustServerCertificate=True;Trusted_Connection=True;MultipleActiveResultSets=true";

    public ApplicationDbContext() { }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Categories => Set<Category>();

    public DbSet<Product> Products => Set<Product>();

    public DbSet<StockTransaction> StockTransactions => Set<StockTransaction>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder
                .UseSqlServer(_connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}