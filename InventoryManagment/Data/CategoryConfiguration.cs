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
    }
}
