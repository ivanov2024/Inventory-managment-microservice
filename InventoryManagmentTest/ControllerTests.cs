using FluentAssertions;
using InventoryManagment.Controllers;
using InventoryManagment.DTOs.Category;
using InventoryManagment.DTOs.Product;
using InventoryManagment.DTOs.StockTransaction;
using InventoryManagment.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace InventoryManagmentTest;

public class ProductsApiControllerTests
{
    private readonly Mock<IProductService> _productServiceMock;
    private readonly ProductsApiController _controller;

    public ProductsApiControllerTests()
    {
        _productServiceMock = new Mock<IProductService>();
        _controller = new ProductsApiController(_productServiceMock.Object);
    }

    [Fact]
    public async Task Create_Returns201Created_WhenProductIsCreated()
    {
        var dto = new ProductCreateUpdateDto
        {
            Name = "Test Product",
            Price = 10.99m,
            Quantity = 50,
            Description = "Test Description",
            CategoryId = 1
        };

        var createdProduct = new ProductDto
        {
            Id = 1,
            Name = "Test Product",
            Price = 10.99m,
            Quantity = 50,
            Description = "Test Description",
            CategoryId = 1
        };

        _productServiceMock.Setup(s => s.CreateProductAsync(dto))
            .ReturnsAsync(createdProduct);

        var result = await _controller.Create(dto);

        var createdResult = result.Result as CreatedAtActionResult;
        createdResult.Should().NotBeNull();
        createdResult!.StatusCode.Should().Be(StatusCodes.Status201Created);
    }

    [Fact]
    public async Task Get_Returns404NotFound_WhenProductDoesNotExist()
    {
        _productServiceMock.Setup(s => s.GetProductByIdAsync(999))
            .ReturnsAsync((ProductDto)null!);

        var result = await _controller.Get(999);

        var notFoundResult = result.Result as NotFoundResult;
        notFoundResult.Should().NotBeNull();
        notFoundResult!.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    public async Task Update_Returns400BadRequest_WhenModelStateIsInvalid()
    {
        var dto = new ProductCreateUpdateDto
        {
            Name = "Test Product",
            Price = 10.99m,
            Quantity = 50,
            Description = "Test Description",
            CategoryId = 1
        };

        _controller.ModelState.AddModelError("Name", "Name is required");

        var result = await _controller.Update(1, dto);

        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult.Should().NotBeNull();
        badRequestResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async Task Update_ThrowsException_WhenProductDoesNotExist()
    {
        var dto = new ProductCreateUpdateDto
        {
            Name = "Test Product",
            Price = 10.99m,
            Quantity = 50,
            Description = "Test Description",
            CategoryId = 1
        };

        _productServiceMock.Setup(s => s.UpdateProductAsync(dto, 999))
            .ThrowsAsync(new Exception("No such product was found!"));

        var act = async () => await _controller.Update(999, dto);

        await act.Should().ThrowAsync<Exception>()
            .WithMessage("No such product was found!");
    }

    [Fact]
    public async Task Delete_Returns204NoContent_WhenProductIsDeleted()
    {
        _productServiceMock.Setup(s => s.DeleteProductAsync(1))
            .ReturnsAsync(true);

        var result = await _controller.Delete(1);

        var noContentResult = result as NoContentResult;
        noContentResult.Should().NotBeNull();
        noContentResult!.StatusCode.Should().Be(StatusCodes.Status204NoContent);
    }
}

public class CategoryApiControllerTests
{
    private readonly Mock<ICategoryService> _categoryServiceMock;
    private readonly CategoryApiController _controller;

    public CategoryApiControllerTests()
    {
        _categoryServiceMock = new Mock<ICategoryService>();
        _controller = new CategoryApiController(_categoryServiceMock.Object);
    }

    [Fact]
    public async Task Create_Returns201Created_WhenCategoryIsCreated()
    {
        var dto = new CategoryCreateUpdateDto
        {
            Name = "Electronics",
            Description = "Electronic devices"
        };

        var createdCategory = new CategoryDto
        {
            Id = 1,
            Name = "Electronics",
            Description = "Electronic devices"
        };

        _categoryServiceMock.Setup(s => s.CreateCategoryAsync(dto))
            .ReturnsAsync(createdCategory);

        var result = await _controller.Create(dto);

        var createdResult = result.Result as CreatedAtActionResult;
        createdResult.Should().NotBeNull();
        createdResult!.StatusCode.Should().Be(StatusCodes.Status201Created);
    }

    [Fact]
    public async Task Get_Returns404NotFound_WhenCategoryDoesNotExist()
    {
        _categoryServiceMock.Setup(s => s.GetCategoryByIdAsync(999))
            .ReturnsAsync((CategoryDto)null!);

        var result = await _controller.Get(999);

        var notFoundResult = result.Result as NotFoundResult;
        notFoundResult.Should().NotBeNull();
        notFoundResult!.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    public async Task Update_Returns400BadRequest_WhenModelStateIsInvalid()
    {
        var dto = new CategoryCreateUpdateDto
        {
            Name = "Electronics",
            Description = "Electronic devices"
        };

        _controller.ModelState.AddModelError("Name", "Name is required");

        var result = await _controller.Update(1, dto);

        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult.Should().NotBeNull();
        badRequestResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async Task Delete_Returns204NoContent_WhenCategoryIsDeleted()
    {
        _categoryServiceMock.Setup(s => s.DeleteCategoryAsync(1))
            .ReturnsAsync(true);

        var result = await _controller.Delete(1);

        var noContentResult = result as NoContentResult;
        noContentResult.Should().NotBeNull();
        noContentResult!.StatusCode.Should().Be(StatusCodes.Status204NoContent);
    }
}

public class StockControllerTests
{
    private readonly Mock<IInventoryService> _inventoryServiceMock;
    private readonly StockController _controller;

    public StockControllerTests()
    {
        _inventoryServiceMock = new Mock<IInventoryService>();
        _controller = new StockController(_inventoryServiceMock.Object);
    }

    [Fact]
    public async Task IncreaseStock_Returns204NoContent_WhenSuccessful()
    {
        var dto = new StockChangeRequestDto
        {
            Amount = 50,
            Reason = "Restock"
        };

        _inventoryServiceMock.Setup(s => s.IncreaseStockAsync(1, dto))
            .Returns(Task.CompletedTask);

        var result = await _controller.IncreaseStock(1, dto);

        var noContentResult = result as NoContentResult;
        noContentResult.Should().NotBeNull();
        noContentResult!.StatusCode.Should().Be(StatusCodes.Status204NoContent);
    }

    [Fact]
    public async Task DecreaseStock_Returns204NoContent_WhenSuccessful()
    {
        var dto = new StockChangeRequestDto
        {
            Amount = 10,
            Reason = "Sale"
        };

        _inventoryServiceMock.Setup(s => s.DecreaseStockAsync(1, dto))
            .Returns(Task.CompletedTask);

        var result = await _controller.DecreaseStock(1, dto);

        var noContentResult = result as NoContentResult;
        noContentResult.Should().NotBeNull();
        noContentResult!.StatusCode.Should().Be(StatusCodes.Status204NoContent);
    }

    [Fact]
    public async Task GetTransactions_Returns200OK_WithTransactions()
    {
        var transactions = new List<StockTransactionDto>
        {
            new StockTransactionDto
            {
                Id = 1,
                ProductId = 1,
                ChangeAmount = 50,
                Reason = "Restock",
                CreatedAt = DateTime.UtcNow
            }
        };

        _inventoryServiceMock.Setup(s => s.GetTransactionsForProductAsync(1))
            .ReturnsAsync(transactions);

        var result = await _controller.GetTransactions(1);

        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);
    }
}
