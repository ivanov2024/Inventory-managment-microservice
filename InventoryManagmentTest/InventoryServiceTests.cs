using FluentAssertions;
using InventoryManagment.Data;
using InventoryManagment.Data.Models;
using InventoryManagment.DTOs.StockTransaction;
using InventoryManagment.Services.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace InventoryManagmentTest;

public class InventoryServiceTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly InventoryService _service;
    private readonly Mock<ILogger<InventoryService>> _loggerMock;
    private readonly Mock<AutoMapper.IMapper> _mapperMock;

    public InventoryServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        _context = new ApplicationDbContext(options);
        _loggerMock = new Mock<ILogger<InventoryService>>();
        _mapperMock = new Mock<AutoMapper.IMapper>();

        _mapperMock.Setup(m => m.Map<StockTransactionDto>(It.IsAny<StockTransaction>()))
            .Returns((StockTransaction st) => new StockTransactionDto
            {
                Id = st.Id,
                ProductId = st.ProductId,
                ChangeAmount = st.ChangeAmount,
                Reason = st.Reason,
                CreatedAt = st.CreatedAt
            });

        _service = new InventoryService(_context, _loggerMock.Object, _mapperMock.Object);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Fact]
    public async Task IncreaseStockAsync_IncreasesProductQuantityCorrectly()
    {
        var category = new Category { Id = 1, Name = "Test Category", Description = "Test Description" };
        var product = new Product
        {
            Id = 1,
            Name = "Test Product",
            Price = 10.99m,
            Quantity = 50,
            Description = "Test Description",
            CategoryId = 1,
            Category = category,
            CreatedAt = DateTime.UtcNow
        };

        _context.Categories.Add(category);
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        var dto = new StockChangeRequestDto { Amount = 20, Reason = "Restock from supplier" };

        await _service.IncreaseStockAsync(1, dto);

        var updatedProduct = await _context.Products.FindAsync(1);
        updatedProduct.Should().NotBeNull();
        updatedProduct!.Quantity.Should().Be(70);
    }

    [Fact]
    public async Task IncreaseStockAsync_CreatesStockTransaction()
    {
        var category = new Category { Id = 1, Name = "Test Category", Description = "Test Description" };
        var product = new Product
        {
            Id = 1,
            Name = "Test Product",
            Price = 10.99m,
            Quantity = 50,
            Description = "Test Description",
            CategoryId = 1,
            Category = category,
            CreatedAt = DateTime.UtcNow
        };

        _context.Categories.Add(category);
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        var dto = new StockChangeRequestDto { Amount = 20, Reason = "Restock from supplier" };

        await _service.IncreaseStockAsync(1, dto);

        var transactions = await _context.StockTransactions.Where(t => t.ProductId == 1).ToListAsync();
        transactions.Should().HaveCount(1);
        transactions[0].ChangeAmount.Should().Be(20);
        transactions[0].Reason.Should().Be("Restock from supplier");
        transactions[0].ProductId.Should().Be(1);
    }

    [Fact]
    public async Task DecreaseStockAsync_DecreasesQuantity()
    {
        var category = new Category { Id = 1, Name = "Test Category", Description = "Test Description" };
        var product = new Product
        {
            Id = 1,
            Name = "Test Product",
            Price = 10.99m,
            Quantity = 50,
            Description = "Test Description",
            CategoryId = 1,
            Category = category,
            CreatedAt = DateTime.UtcNow
        };

        _context.Categories.Add(category);
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        var dto = new StockChangeRequestDto { Amount = 15, Reason = "Customer purchase" };

        await _service.DecreaseStockAsync(1, dto);

        var updatedProduct = await _context.Products.FindAsync(1);
        updatedProduct.Should().NotBeNull();
        updatedProduct!.Quantity.Should().Be(35);
    }

    [Fact]
    public async Task DecreaseStockAsync_CreatesStockTransaction()
    {
        var category = new Category { Id = 1, Name = "Test Category", Description = "Test Description" };
        var product = new Product
        {
            Id = 1,
            Name = "Test Product",
            Price = 10.99m,
            Quantity = 50,
            Description = "Test Description",
            CategoryId = 1,
            Category = category,
            CreatedAt = DateTime.UtcNow
        };

        _context.Categories.Add(category);
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        var dto = new StockChangeRequestDto { Amount = 15, Reason = "Customer purchase" };

        await _service.DecreaseStockAsync(1, dto);

        var transactions = await _context.StockTransactions.Where(t => t.ProductId == 1).ToListAsync();
        transactions.Should().HaveCount(1);
        transactions[0].ChangeAmount.Should().Be(-15);
        transactions[0].Reason.Should().Be("Customer purchase");
        transactions[0].ProductId.Should().Be(1);
    }

    [Fact]
    public async Task DecreaseStockAsync_ThrowsIfInsufficientStock()
    {
        var category = new Category { Id = 1, Name = "Test Category", Description = "Test Description" };
        var product = new Product
        {
            Id = 1,
            Name = "Test Product",
            Price = 10.99m,
            Quantity = 10,
            Description = "Test Description",
            CategoryId = 1,
            Category = category,
            CreatedAt = DateTime.UtcNow
        };

        _context.Categories.Add(category);
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        var dto = new StockChangeRequestDto { Amount = 20, Reason = "Customer purchase" };

        var act = async () => await _service.DecreaseStockAsync(1, dto);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Insufficient stock");

        var unchangedProduct = await _context.Products.FindAsync(1);
        unchangedProduct!.Quantity.Should().Be(10);
    }

    [Fact]
    public async Task GetTransactionsForProductAsync_ReturnsTransactionsOrderedByCreatedAtDesc()
    {
        var category = new Category { Id = 1, Name = "Test Category", Description = "Test Description" };
        var product = new Product
        {
            Id = 1,
            Name = "Test Product",
            Price = 10.99m,
            Quantity = 100,
            Description = "Test Description",
            CategoryId = 1,
            Category = category,
            CreatedAt = DateTime.UtcNow
        };

        _context.Categories.Add(category);
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        var transaction1 = new StockTransaction
        {
            ProductId = 1,
            ChangeAmount = 50,
            Reason = "Initial stock",
            CreatedAt = DateTime.UtcNow.AddDays(-3)
        };

        var transaction2 = new StockTransaction
        {
            ProductId = 1,
            ChangeAmount = 30,
            Reason = "Restock",
            CreatedAt = DateTime.UtcNow.AddDays(-1)
        };

        var transaction3 = new StockTransaction
        {
            ProductId = 1,
            ChangeAmount = -10,
            Reason = "Sale",
            CreatedAt = DateTime.UtcNow
        };

        _context.StockTransactions.AddRange(transaction1, transaction2, transaction3);
        await _context.SaveChangesAsync();

        var result = await _service.GetTransactionsForProductAsync(1);

        var resultList = result.ToList();
        resultList.Should().HaveCount(3);
        resultList[0].Reason.Should().Be("Sale");
        resultList[1].Reason.Should().Be("Restock");
        resultList[2].Reason.Should().Be("Initial stock");
    }

    [Fact]
    public async Task GetTransactionsForProductAsync_OnlyReturnsTransactionsForThatProduct()
    {
        var category = new Category { Id = 1, Name = "Test Category", Description = "Test Description" };
        var product1 = new Product
        {
            Id = 1,
            Name = "Product 1",
            Price = 10.99m,
            Quantity = 100,
            Description = "Test Description",
            CategoryId = 1,
            Category = category,
            CreatedAt = DateTime.UtcNow
        };

        var product2 = new Product
        {
            Id = 2,
            Name = "Product 2",
            Price = 20.99m,
            Quantity = 50,
            Description = "Test Description",
            CategoryId = 1,
            Category = category,
            CreatedAt = DateTime.UtcNow
        };

        _context.Categories.Add(category);
        _context.Products.AddRange(product1, product2);
        await _context.SaveChangesAsync();

        var transaction1 = new StockTransaction
        {
            ProductId = 1,
            ChangeAmount = 50,
            Reason = "Product 1 stock",
            CreatedAt = DateTime.UtcNow
        };

        var transaction2 = new StockTransaction
        {
            ProductId = 2,
            ChangeAmount = 30,
            Reason = "Product 2 stock",
            CreatedAt = DateTime.UtcNow
        };

        var transaction3 = new StockTransaction
        {
            ProductId = 1,
            ChangeAmount = -10,
            Reason = "Product 1 sale",
            CreatedAt = DateTime.UtcNow
        };

        _context.StockTransactions.AddRange(transaction1, transaction2, transaction3);
        await _context.SaveChangesAsync();

        var result = await _service.GetTransactionsForProductAsync(1);

        var resultList = result.ToList();
        resultList.Should().HaveCount(2);
        resultList.Should().OnlyContain(t => t.ProductId == 1);
        resultList.Should().Contain(t => t.Reason == "Product 1 stock");
        resultList.Should().Contain(t => t.Reason == "Product 1 sale");
        resultList.Should().NotContain(t => t.Reason == "Product 2 stock");
    }
}
