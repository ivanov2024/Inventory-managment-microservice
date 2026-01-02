using InventoryManagment.Data.Models;
using InventoryManagment.Models;

namespace InventoryManagment.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();

        Task<Category> GetCategoryByIdAsync(int categoryId);

        Task<bool> CreateCategoryAsync(CategoryViewModel categoryViewModel);

        Task<bool> UpdateCategoryAsync(CategoryViewModel categoryViewModel, int categoryId);

        Task<bool> DeleteCategoryAsync(int categoryId);
    }
}
