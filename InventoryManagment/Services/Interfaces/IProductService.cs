using InventoryManagment.Data.Models;
using InventoryManagment.Models;

namespace InventoryManagment.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductViewModel>> GetAllProductsAsync();

        Task<Product> GetProductById(int productId);
    }
}
