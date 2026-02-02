using AutoMapper;
using InventoryManagment.Data;
using InventoryManagment.Data.Models;
using InventoryManagment.DTOs.StockTransaction;
using InventoryManagment.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagment.Services.Implementations
{
    public class InventoryService : IInventoryService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<InventoryService> _logger;
        private readonly IMapper _mapper;

        public InventoryService(ApplicationDbContext dbContext, ILogger<InventoryService> logger, IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task IncreaseStockAsync(int productId, StockChangeRequestDto dto)
        {
            try
            {
                var product =
                     await _dbContext
                     .Products
                     .FirstOrDefaultAsync(p => p.Id == productId) 
                     ?? throw new KeyNotFoundException("Product not found");

                using var transaction = 
                    await _dbContext
                    .Database
                    .BeginTransactionAsync();

                product.Quantity += dto.Amount;

                CreateStockTransaction(productId, dto.Amount, dto.Reason);

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error increasing stock for product {ProductId}", productId);
                throw;
            }
        }

        public async Task DecreaseStockAsync(int productId, StockChangeRequestDto dto)
        {
            try
            {
                var product =
                     await _dbContext
                     .Products
                     .FirstOrDefaultAsync(p => p.Id == productId)
                     ?? throw new KeyNotFoundException("Product not found");

                if (product.Quantity < dto.Amount)
                {
                    throw new InvalidOperationException("Insufficient stock");
                }

                using var transaction = 
                    await _dbContext
                    .Database
                    .BeginTransactionAsync();

                product.Quantity -= dto.Amount;

                CreateStockTransaction(productId, -dto.Amount, dto.Reason);

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error decreasing stock for product {ProductId}", productId);
                throw;
            }
        }

        public async Task<IEnumerable<StockTransactionDto>> GetTransactionsForProductAsync(int productId)
        {
            try
            {
                return await _dbContext
                    .StockTransactions
                    .AsNoTracking()
                    .Where(t => t.ProductId == productId)
                    .OrderByDescending(t => t.CreatedAt)
                    .Select(t => _mapper.Map<StockTransactionDto>(t))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while fetching the transactions");
                throw;
            }
        }

        private void CreateStockTransaction(int productId, int changeAmount, string reason)
        {
            var transaction = new StockTransaction
            {
                ProductId = productId,
                ChangeAmount = changeAmount,
                Reason = reason,
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.StockTransactions.Add(transaction);
        }
    }
}
