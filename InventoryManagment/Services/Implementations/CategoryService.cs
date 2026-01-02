using InventoryManagment.Data;
using InventoryManagment.Data.Models;
using InventoryManagment.Models;
using InventoryManagment.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace InventoryManagment.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger _logger;

        public CategoryService(ApplicationDbContext dbContext, ILogger logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            try
            {
                 IEnumerable<Category> categories 
                    = await _dbContext
                    .Categories
                    .AsNoTracking()
                    .ToListAsync();

                return categories;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error fetching all categories");
                throw;
            }
        }

        public async Task<Category> GetCategoryByIdAsync(int categoryId)
        {
            try
            {
                Category category = 
                    await _dbContext
                    .Categories
                    .AsNoTracking()
                    .FirstAsync(c => c.Id == categoryId);

                return category;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error fetching a category by ID {CategoryId}", categoryId);
                throw;
            }
        }

        public async Task<bool> CreateCategoryAsync(CategoryViewModel categoryViewModel)
        {
            try
            {
                Category category =
                    new()
                    {
                        Name = categoryViewModel.Name,
                        Description = categoryViewModel.Description,
                    };

                await _dbContext
                    .Categories
                    .AddAsync(category);

                await _dbContext
                    .SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating a category");
                throw;
            }
        }

        public async Task<bool> UpdateCategoryAsync(CategoryViewModel categoryViewModel, int categoryId)
        {
            try
            {
                var category =
                    await _dbContext
                    .Categories
                    .FirstOrDefaultAsync(c => c.Id == categoryId) 
                    ?? throw new Exception("No such category was found!");

                category.Name = categoryViewModel.Name;
                category.Description = categoryViewModel.Description;

                await 
                    _dbContext
                    .SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating category with ID {CategoryId}", categoryId);
                throw;
            }
        }

        public async Task<bool> DeleteCategoryAsync(int categoryId)
        {
            try
            {
                var category =
                    await _dbContext
                    .Categories
                    .FirstOrDefaultAsync(c => c.Id == categoryId)
                    ?? throw new Exception("No such category was found!");

                _dbContext
                    .Categories
                    .Remove(category);

                await _dbContext
                    .SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting the product");
                throw;
            }
        }
    }
}
