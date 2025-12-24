using InventoryManagment.Data.Models;

namespace InventoryManagment.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();

        Task<Product> GetProductById(int productId);
    }
}
