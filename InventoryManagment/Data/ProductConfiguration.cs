using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InventoryManagment.Data.Models;

namespace InventoryManagment.Data
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> product)
        {
            product
                .HasKey(p => p.Id);

            product
                .Property(p => p.Price)
                .HasColumnType("decimal(18, 2)");

            product
                .ToTable("Products", t =>
                {
                    t
                    .HasCheckConstraint("CK_Product_Price_Range", "Price >= 0.00 AND Price <= 9999.00");
                    t
                    .HasCheckConstraint("CK_Product_Quantity_Range", "Quantity >= 0 AND Quantity <= 9999");
                });

            product
                 .Property(p => p.CreatedAt)
                 .HasDefaultValue(DateTime.UtcNow)
                 .IsRequired();

            product
                 .Property(p => p.UpdatedAt)
                 .HasDefaultValue(null)
                 .IsRequired(false);

            product
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            product
                .HasQueryFilter(static p => p.Category != null);
        }
    }
}
