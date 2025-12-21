using InventoryManagment.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagment.Data;

public class StockTransactionsConfiguration : IEntityTypeConfiguration<StockTransaction>
{
    public void Configure(EntityTypeBuilder<StockTransaction> stockTransaction)
    {
        stockTransaction
            .HasKey(st => st.Id);

        stockTransaction
            .Property(st => st.ChangeAmount)
            .IsRequired();

        stockTransaction
            .Property(st => st.Reason)
            .HasMaxLength(150)
            .IsRequired();

        stockTransaction
            .Property(st => st.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();

        stockTransaction
            .HasOne(st => st.Product)
            .WithMany(p => p.StockTransactions)
            .HasForeignKey(st => st.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        stockTransaction
            .ToTable("StockTransactions", t =>
            {
                t.HasCheckConstraint("CK_StockTransaction_ChangeAmount_NonZero", "ChangeAmount <> 0");
            });
    }
}
