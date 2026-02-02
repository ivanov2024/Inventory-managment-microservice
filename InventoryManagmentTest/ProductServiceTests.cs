using FluentAssertions;
using InventoryManagment.Data;
using InventoryManagment.Data.Models;
using InventoryManagment.DTOs.Product;
using InventoryManagment.Services.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace InventoryManagmentTest;

public class ProductServiceTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly ProductService _service;
    private readonly Mock<ILogger<ProductService>> _loggerMock;
    private readonly Mock<AutoMapper.IMapper> _mapperMock;

    public ProductServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _loggerMock = new Mock<ILogger<ProductService>>();
        _mapperMock = new Mock<AutoMapper.IMapper>();

        _mapperMock.Setup(m => m.Map<Product>(It.IsAny<ProductCreateUpdateDto>()))
            .Returns((ProductCreateUpdateDto dto) => new Product
            {
                Name = dto.Name,
                Price = dto.Price,
                Quantity = dto.Quantity,
                Description = dto.Description,
                CategoryId = dto.CategoryId,
                CreatedAt = DateTime.UtcNow
            });

        _mapperMock.Setup(m => m.Map<ProductDto>(It.IsAny<Product>()))
            .Returns((Product p) => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Quantity = p.Quantity,
                Description = p.Description,
                CategoryId = p.CategoryId
            });

        _service = new ProductService(_context, _loggerMock.Object, _mapperMock.Object);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Fact]
    public async Task UpdateProductAsync_ThrowsException_WhenProductNotFound()
    {
        var dto = new ProductCreateUpdateDto
        {
            Name = "Updated Product",
            Price = 15.99m,
            Quantity = 100,
            Description = "Updated Description",
            CategoryId = 1
        };

        var act = async () => await _service.UpdateProductAsync(dto, 999);

        await act.Should().ThrowAsync<Exception>()
            .WithMessage("No such product was found!");
    }

    [Fact]
    public async Task UpdateProductAsync_DoesNotModifyQuantity()
    {
        var category = new Category { Id = 1, Name = "Test Category", Description = "Test Description" };
        var product = new Product
        {
            Id = 1,
            Name = "Original Product",
            Price = 10.99m,
            Quantity = 50,
            Description = "Original Description",
            CategoryId = 1,
            Category = category,
            CreatedAt = DateTime.UtcNow
        };

        _context.Categories.Add(category);
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        var dto = new ProductCreateUpdateDto
        {
            Name = "Updated Product",
            Price = 15.99m,
            Quantity = 200,
            Description = "Updated Description",
            CategoryId = 1
        };

        await _service.UpdateProductAsync(dto, 1);

        var updatedProduct = await _context.Products.FindAsync(1);
        updatedProduct.Should().NotBeNull();
        updatedProduct!.Quantity.Should().Be(50);
        updatedProduct.Name.Should().Be("Updated Product");
        updatedProduct.Price.Should().Be(15.99m);
        updatedProduct.Description.Should().Be("Updated Description");
    }

    [Fact]
    public async Task UpdateProductAsync_ValidatesCategoryExists()
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

        var dto = new ProductCreateUpdateDto
        {
            Name = "Updated Product",
            Price = 15.99m,
            Quantity = 100,
            Description = "Updated Description",
            CategoryId = 999
        };

        var act = async () => await _service.UpdateProductAsync(dto, 1);

        await act.Should().ThrowAsync<Exception>()
            .WithMessage("No such category was found!");
    }

    [Fact]
    public async Task UpdateProductAsync_UpdatesProductSuccessfully()
    {
        var category1 = new Category { Id = 1, Name = "Category 1", Description = "Description 1" };
        var category2 = new Category { Id = 2, Name = "Category 2", Description = "Description 2" };
        var product = new Product
        {
            Id = 1,
            Name = "Original Product",
            Price = 10.99m,
            Quantity = 50,
            Description = "Original Description",
            CategoryId = 1,
            Category = category1,
            CreatedAt = DateTime.UtcNow
        };

        _context.Categories.AddRange(category1, category2);
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        var dto = new ProductCreateUpdateDto
        {
            Name = "Updated Product",
            Price = 25.99m,
            Quantity = 200,
            Description = "Updated Description",
            CategoryId = 2
        };

        var result = await _service.UpdateProductAsync(dto, 1);

        result.Should().BeTrue();
        var updatedProduct = await _context.Products.FindAsync(1);
        updatedProduct.Should().NotBeNull();
        updatedProduct!.Name.Should().Be("Updated Product");
        updatedProduct.Price.Should().Be(25.99m);
        updatedProduct.Quantity.Should().Be(50);
        updatedProduct.Description.Should().Be("Updated Description");
        updatedProduct.CategoryId.Should().Be(2);
        updatedProduct.UpdatedAt.Should().NotBeNull();
    }
}
