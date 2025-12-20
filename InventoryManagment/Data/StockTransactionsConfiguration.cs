using InventoryManagment.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagment.Data
{
    public class StockTransactionsConfiguration : IEntityTypeConfiguration<StockTransaction>
    {
        public void Configure(EntityTypeBuilder<StockTransaction> stockTransaction)
        {
            stockTransaction
                .HasKey(st => st.Id);

            stockTransaction
                .HasOne(st => st.Product)
                .WithMany(p => p.StockTransactions)
                .HasForeignKey(st => st.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            stockTransaction
                .HasQueryFilter(st => st.Product != null);

            stockTransaction
                .Property(st => st.CreatedAt)
                .HasDefaultValue(DateTime.UtcNow)
                .IsRequired();
        }
    }
}
