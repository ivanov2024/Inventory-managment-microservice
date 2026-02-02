using System.ComponentModel.DataAnnotations;

namespace InventoryManagment.DTOs.StockTransaction
{
    /// <summary>
    /// Read-only DTO for queries, representing a stock transaction.
    /// </summary>
    public class StockTransactionDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ChangeAmount { get; set; }
        public string Reason { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
