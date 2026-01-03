using InventoryManagment.DTOs.Category;

namespace InventoryManagment.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();

        Task<CategoryDto> GetCategoryByIdAsync(int categoryId);

        Task<IEnumerable<CategoryWithProductsDto>> GetCategoriesWithProductsAsync();

        Task<CategoryDto> CreateCategoryAsync(CategoryCreateUpdateDto categoryDto);

        Task<bool> UpdateCategoryAsync(CategoryCreateUpdateDto categoryDto, int categoryId);

        Task<bool> DeleteCategoryAsync(int categoryId);
    }
}
