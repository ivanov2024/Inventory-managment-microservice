using InventoryManagment.Data;
using InventoryManagment.Data.Models;
using InventoryManagment.DTOs.Product;
using InventoryManagment.Models;
using InventoryManagment.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagment.Services.Implementations
{
    public class ProductService : IProductService
    {
        const string _dateFormat = "dddd-MMMM-yyyy";

        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<ProductService> _logger;

        public ProductService(ApplicationDbContext dbContext, ILogger<ProductService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            try
            {
                 IEnumerable<ProductDto> products 
                    = await _dbContext
                    .Products
                    .AsNoTracking()
                    .Select(p => new ProductDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Price = p.Price,
                        Quantity = p.Quantity,
                        Description = p.Description,
                        CreatedAt = p.CreatedAt.ToString(_dateFormat),
                        UpdatedAt = p.UpdatedAt!.Value.ToString(_dateFormat),
                        CategoryId = p.CategoryId,
                        Category = p.Category.Name
                    })
                    .ToListAsync();

                return products;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error fetching all products");
                throw;
            }
        }

        public async Task<ProductDto> GetProductByIdAsync(int productId)
        {
            try
            {
                ProductDto product = 
                     _dbContext
                    .Products
                    .AsNoTracking()
                    .Where(p => p.Id == productId)
                    .Select(p => new ProductDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Price = p.Price,
                        Quantity = p.Quantity,
                        Description = p.Description,
                        CreatedAt = p.CreatedAt.ToString(_dateFormat),
                        UpdatedAt = p.UpdatedAt!.Value.ToString(_dateFormat),
                        CategoryId = p.CategoryId,
                        Category = p.Category.Name
                    })
                    .Single();

                return product;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error fetching product by ID {ProductId}", productId);
                throw;
            }
        }

        public async Task<Product> CreateProductAsync(ProductViewModel productViewModel)
        {
            try
            {
                Product product = 
                    new()
                    {
                        Name = productViewModel.Name,
                        Price = productViewModel.Price,
                        Quantity = productViewModel.Quantity,
                        Description = productViewModel.Description,                        CategoryId = productViewModel.CategoryId
                    };

                await _dbContext
                    .Products
                    .AddAsync(product);

                await _dbContext
                    .SaveChangesAsync();

                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating a product");
                throw;
            }
        }

        public async Task<bool> UpdateProductAsync(ProductViewModel productViewModel, int productId)
        {
            try
            {
                var product =
                    await _dbContext
                    .Products
                    .FirstOrDefaultAsync(p => p.Id == productId) 
                    ?? throw new Exception("No such product was found!");

                bool categoryExists =
                   await _dbContext
                   .Categories
                   .AnyAsync(c => c.Id == productViewModel.CategoryId);

                if (!categoryExists) throw new Exception("No such category was found!");

                product.Name = productViewModel.Name;
                product.Price = productViewModel.Price;
                product.Quantity = productViewModel.Quantity;
                product.Description = productViewModel.Description;
                product.CategoryId = productViewModel.CategoryId;
                product.UpdatedAt = DateTime.UtcNow;

                await 
                    _dbContext
                    .SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product with ID {0}", productId);
                throw;
            }
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            try
            {
                var product =
                    await _dbContext
                    .Products
                    .FirstOrDefaultAsync(p => p.Id == productId)
                    ?? throw new Exception("No such product was found!");

                _dbContext
                    .Products
                    .Remove(product);

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
