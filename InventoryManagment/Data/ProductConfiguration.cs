using InventoryManagment.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagment.Data;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> product)
    {
        product
            .HasKey(p => p.Id);

        product
            .Property(p => p.Name)
            .HasMaxLength(100)
            .IsRequired();

        product
            .Property(p => p.Description)
            .HasMaxLength(150)
            .IsRequired();

        product
            .Property(p => p.Price)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        product
            .Property(p => p.Quantity)
            .HasDefaultValue(0)
            .IsRequired();

        product
            .Property(p => p.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();

        product
            .Property(p => p.UpdatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAddOrUpdate();

        product
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        product
            .HasMany(p => p.StockTransactions)
            .WithOne(st => st.Product)
            .HasForeignKey(st => st.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        product.ToTable("Products", t =>
        {
            t.HasCheckConstraint("CK_Product_Price_Range", "Price BETWEEN 0 AND 9999");
            t.HasCheckConstraint("CK_Product_Quantity_Range", "Quantity BETWEEN 0 AND 9999");
        });

        product.HasData(
            new Product
            {
                Id = 1,
                Name = "iPhone 15",
                Price = 1199.99m,
                CategoryId = 1,
                Description = "Latest Apple smartphone with A17 chip, 6.7-inch display, and advanced camera system."
            },
            new Product
            {
                Id = 2,
                Name = "Samsung Galaxy S24",
                Price = 1099.99m,
                CategoryId = 1,
                Description = "Flagship Samsung phone with cutting-edge features and high-resolution AMOLED display."
            },
            new Product
            {
                Id = 3,
                Name = "C# Programming Book",
                Price = 49.99m,
                CategoryId = 2,
                Description = "Comprehensive guide to C# programming for beginners and professionals."
            },
            new Product
            {
                Id = 4,
                Name = "Men's T-Shirt",
                Price = 19.99m,
                CategoryId = 3,
                Description = "Comfortable cotton t-shirt, available in multiple colors and sizes."
            },
            new Product
            {
                Id = 5,
                Name = "Blender",
                Price = 89.99m,
                CategoryId = 4,
                Description = "High-power kitchen blender perfect for smoothies, soups, and sauces."
            },
            new Product
            {
                Id = 6,
                Name = "Coffee Maker",
                Price = 129.99m,
                CategoryId = 4,
                Description = "Automatic coffee maker with programmable timer and multiple brewing options."
            }
        );
    }
}
