using InventoryManagment.Data;
using InventoryManagment.Data.Models;
using InventoryManagment.DTOs.Category;
using InventoryManagment.DTOs.Product;
using InventoryManagment.Models;
using InventoryManagment.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace InventoryManagment.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(ApplicationDbContext dbContext, ILogger<CategoryService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            try
            {
                 IEnumerable<CategoryDto> categories 
                    = await _dbContext
                    .Categories
                    .AsNoTracking()
                    .Select(c => new CategoryDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description,
                    })
                    .ToListAsync();

                return categories;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error fetching all categories");
                throw;
            }
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(int categoryId)
        {
            try
            {
                CategoryDto category =
                     _dbContext
                    .Categories
                    .AsNoTracking()
                    .Where(c => c.Id == categoryId)
                    .Select(c => new CategoryDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description,
                    })
                    .Single();

                return category;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error fetching a category by ID {CategoryId}", categoryId);
                throw;
            }
        }

        public async Task<IEnumerable<CategoryWithProductsDto>> GetCategoriesWithProductsAsync()
        {
            try
            {
                IEnumerable<CategoryWithProductsDto> categories
                    = await _dbContext
                    .Categories
                    .Include(c => c.Products)
                    .AsNoTracking()
                    .OrderBy(c => c.Id)
                    .Select(c => new CategoryWithProductsDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description,
                        Products = c.Products.Select(p => new ProductDto
                        {
                            Id = p.Id,
                            Name = p.Name,
                            Price = p.Price,
                            Quantity = p.Quantity,
                            Description = p.Description,
                            CategoryId = p.CategoryId,
                            Category = c.Name,
                        }).ToList()
                    })
                    .ToListAsync();

                return categories;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all categories and their products");
                throw;
            }
        }

        public async Task<Category> CreateCategoryAsync(CategoryViewModel categoryViewModel)
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

                return category;
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
