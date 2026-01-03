using InventoryManagment.Data.Models;
using InventoryManagment.DTOs.Category;
using InventoryManagment.Models;

namespace InventoryManagment.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();

        Task<CategoryDto> GetCategoryByIdAsync(int categoryId);

        Task<IEnumerable<CategoryWithProductsDto>> GetCategoriesWithProductsAsync();

        Task<Category> CreateCategoryAsync(CategoryViewModel categoryViewModel);

        Task<bool> UpdateCategoryAsync(CategoryViewModel categoryViewModel, int categoryId);

        Task<bool> DeleteCategoryAsync(int categoryId);
    }
}
