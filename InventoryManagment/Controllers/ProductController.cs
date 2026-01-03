using InventoryManagment.DTOs.Product;
using InventoryManagment.Models;
using InventoryManagment.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagment.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
            => _productService = productService;

        private static ProductViewModel ToViewModel(ProductDto product)
            => new()
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Quantity = product.Quantity,
                Description = product.Description,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt,
                CategoryId = product.CategoryId,
            };

        // Map ProductViewModel to ProductCreateUpdateDto
        private static ProductCreateUpdateDto ToCreateUpdateDto(ProductViewModel productViewModel)
            => new()
            {
                Name = productViewModel.Name,
                Price = productViewModel.Price,
                Quantity = productViewModel.Quantity,
                Description = productViewModel.Description,
                CategoryId = productViewModel.CategoryId,
            };

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllProductsAsync();

            var productsToViewModel = products
                .Select(ToViewModel)
                .ToList();

            return View(productsToViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetProductById(int productId)
        {
            var product = await _productService.GetProductByIdAsync(productId);

            var productViewModel = ToViewModel(product);

            return View(productViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductViewModel productViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(productViewModel);
            }

            var createdProduct = await _productService
                .CreateProductAsync(ToCreateUpdateDto(productViewModel));

            if (createdProduct is null)
            {
                ModelState
                    .AddModelError(string.Empty, "Error creating a product");

                return View(productViewModel);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProduct(ProductViewModel productViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(productViewModel);
            }

            var updatedProduct = await _productService
                .UpdateProductAsync(ToCreateUpdateDto(productViewModel), productViewModel.Id);

            if (!updatedProduct)
            {
                ModelState
                    .AddModelError(string.Empty, "Error updating the product");

                return View(productViewModel);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var deletedProduct
                = await _productService
                .DeleteProductAsync(productId);

            if (!deletedProduct)
            {
                ModelState
                    .AddModelError(string.Empty, "Error deleting the product");

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}