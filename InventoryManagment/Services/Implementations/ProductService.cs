using InventoryManagment.Data;
using InventoryManagment.Data.Models;
using InventoryManagment.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagment.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger _logger;

        public ProductService(ApplicationDbContext dbContext, ILogger logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            try
            {
                 IEnumerable<Product> products 
                    = await _dbContext
                    .Products
                    .AsNoTracking()
                    .ToListAsync();

                return products;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error fetching all products");
                throw;
            }
        }

        public async Task<Product> GetProductById(int productId)
        {
            try
            {
                Product product = 
                    await _dbContext
                    .Products
                    .AsNoTracking()
                    .FirstAsync(p => p.Id == productId);

                return product;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error fetching product by ID {ProductId}", productId);
                throw;
            }
        }
    }
}
