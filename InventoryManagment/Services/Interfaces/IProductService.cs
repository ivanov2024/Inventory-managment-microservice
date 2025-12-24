using InventoryManagment.Data.Models;
using InventoryManagment.Models;

namespace InventoryManagment.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();

        Task<Product> GetProductByIdAsync(int productId);

        Task<Product> CreateProductAsync(ProductViewModel productViewModel);
    }
}
