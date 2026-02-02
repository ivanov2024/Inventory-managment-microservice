using FluentAssertions;
using InventoryManagment.Data;
using InventoryManagment.Data.Models;
using InventoryManagment.DTOs.Category;
using InventoryManagment.Services.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace InventoryManagmentTest;

public class CategoryServiceTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly CategoryService _service;
    private readonly Mock<ILogger<CategoryService>> _loggerMock;
    private readonly Mock<AutoMapper.IMapper> _mapperMock;

    public CategoryServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _loggerMock = new Mock<ILogger<CategoryService>>();
        _mapperMock = new Mock<AutoMapper.IMapper>();

        _mapperMock.Setup(m => m.Map<Category>(It.IsAny<CategoryCreateUpdateDto>()))
            .Returns((CategoryCreateUpdateDto dto) => new Category
            {
                Name = dto.Name,
                Description = dto.Description
            });

        _mapperMock.Setup(m => m.Map<CategoryDto>(It.IsAny<Category>()))
            .Returns((Category c) => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            });

        _service = new CategoryService(_context, _loggerMock.Object, _mapperMock.Object);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Fact]
    public async Task CreateCategoryAsync_CreatesCategory()
    {
        var dto = new CategoryCreateUpdateDto
        {
            Name = "Electronics",
            Description = "Electronic devices and accessories"
        };

        var result = await _service.CreateCategoryAsync(dto);

        var category = await _context.Categories.FirstOrDefaultAsync();
        category.Should().NotBeNull();
        category!.Name.Should().Be("Electronics");
        category.Description.Should().Be("Electronic devices and accessories");
    }

    [Fact]
    public async Task UpdateCategoryAsync_ThrowsException_WhenNotFound()
    {
        var dto = new CategoryCreateUpdateDto
        {
            Name = "Updated Category",
            Description = "Updated Description"
        };

        var act = async () => await _service.UpdateCategoryAsync(dto, 999);

        await act.Should().ThrowAsync<Exception>()
            .WithMessage("No such category was found!");
    }

    [Fact]
    public async Task UpdateCategoryAsync_UpdatesCategorySuccessfully()
    {
        var category = new Category
        {
            Id = 1,
            Name = "Original Category",
            Description = "Original Description"
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        var dto = new CategoryCreateUpdateDto
        {
            Name = "Updated Category",
            Description = "Updated Description"
        };

        var result = await _service.UpdateCategoryAsync(dto, 1);

        result.Should().BeTrue();
        var updatedCategory = await _context.Categories.FindAsync(1);
        updatedCategory.Should().NotBeNull();
        updatedCategory!.Name.Should().Be("Updated Category");
        updatedCategory.Description.Should().Be("Updated Description");
    }

    [Fact]
    public async Task DeleteCategoryAsync_RemovesEntity()
    {
        var category = new Category
        {
            Id = 1,
            Name = "Test Category",
            Description = "Test Description"
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        var result = await _service.DeleteCategoryAsync(1);

        result.Should().BeTrue();
        var deletedCategory = await _context.Categories.FindAsync(1);
        deletedCategory.Should().BeNull();
    }

    [Fact]
    public async Task DeleteCategoryAsync_ThrowsException_WhenNotFound()
    {
        var act = async () => await _service.DeleteCategoryAsync(999);

        await act.Should().ThrowAsync<Exception>()
            .WithMessage("No such category was found!");
    }
}
