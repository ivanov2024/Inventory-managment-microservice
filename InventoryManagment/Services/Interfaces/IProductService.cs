using InventoryManagment.DTOs.Product;

namespace InventoryManagment.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();

        Task<ProductDto> GetProductByIdAsync(int productId);

        Task<ProductDto> CreateProductAsync(ProductCreateUpdateDto productDto);

        Task<bool> UpdateProductAsync(ProductCreateUpdateDto productDto, int productId);

        Task<bool> DeleteProductAsync(int productId);
    }
}
