using InventoryManagment.Data.Models;
using InventoryManagment.DTOs.Product;
using InventoryManagment.Models;

namespace InventoryManagment.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();

        Task<ProductDto> GetProductByIdAsync(int productId);

        Task<Product> CreateProductAsync(ProductViewModel productViewModel);

        Task<bool> UpdateProductAsync(ProductViewModel productViewModel, int productId);

        Task<bool> DeleteProductAsync(int productId);
    }
}
