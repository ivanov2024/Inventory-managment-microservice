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
    }
}
