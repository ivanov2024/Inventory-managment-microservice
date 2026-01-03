using AutoMapper;
using InventoryManagment.Data;
using InventoryManagment.Data.Models;
using InventoryManagment.DTOs.Category;
using InventoryManagment.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagment.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<CategoryService> _logger;
        private readonly IMapper _mapper;

        public CategoryService(ApplicationDbContext dbContext, ILogger<CategoryService> logger, IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            try
            {
                 var categories 
                    = await _dbContext
                    .Categories
                    .AsNoTracking()
                    .ToListAsync();

                return _mapper.Map<IEnumerable<CategoryDto>>(categories);
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
                var category =
                     await _dbContext
                    .Categories
                    .AsNoTracking()
                    .FirstAsync(c => c.Id == categoryId);

                return _mapper.Map<CategoryDto>(category);
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
                var categories
                    = await _dbContext
                    .Categories
                    .Include(c => c.Products)
                    .AsNoTracking()
                    .ToListAsync();

                return _mapper.Map<IEnumerable<CategoryWithProductsDto>>(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all categories and their products");
                throw;
            }
        }

        public async Task<CategoryDto> CreateCategoryAsync(CategoryCreateUpdateDto categoryDto)
        {
            try
            {
                var category = _mapper.Map<Category>(categoryDto);

                await _dbContext
                    .Categories
                    .AddAsync(category);

                await _dbContext
                    .SaveChangesAsync();

                return _mapper.Map<CategoryDto>(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating a category");
                throw;
            }
        }

        public async Task<bool> UpdateCategoryAsync(CategoryCreateUpdateDto categoryDto, int categoryId)
        {
            try
            {
                var category =
                    await _dbContext
                    .Categories
                    .FirstOrDefaultAsync(c => c.Id == categoryId) 
                    ?? throw new Exception("No such category was found!");

                category.Name = categoryDto.Name;
                category.Description = categoryDto.Description;

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
