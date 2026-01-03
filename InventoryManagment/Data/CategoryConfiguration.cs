using InventoryManagment.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagment.Data;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> category)
    {
        category
            .HasKey(c => c.Id);

        category
            .HasIndex(c => c.Name)
            .IsUnique();

        category
            .Property(c => c.Name)
            .HasMaxLength(50)
            .IsRequired();

        category
            .Property(c => c.Description)
            .HasMaxLength(150)
            .IsRequired();

        category
            .HasMany(c => c.Products)
            .WithOne(p => p.Category)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        category.HasData(
            new Category
            {
                Id = 1,
                Name = "Electronics",
                Description = "Phones, laptops, TVs and more"  
            },
            new Category
            {
                Id = 2,
                Name = "Books",
                Description = "Fiction, non-fiction, and educational books"
            },
            new Category
            {
                Id = 3,
                Name = "Clothing",
                Description = "Men's and women's apparel"
            },
            new Category 
            { 
                Id = 4,
                Name = "Home & Kitchen", 
                Description = "Appliances, furniture, and kitchenware" 
            }
        );
    }
}
