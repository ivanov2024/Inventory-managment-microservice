using InventoryManagment.DTOs.StockTransaction;

namespace InventoryManagment.Services.Interfaces
{
    public interface IInventoryService
    {
        Task IncreaseStockAsync(int productId, StockChangeRequestDto dto);
        Task DecreaseStockAsync(int productId, StockChangeRequestDto dto);
        Task<IEnumerable<StockTransactionDto>> GetTransactionsForProductAsync(int productId);
    }
}
