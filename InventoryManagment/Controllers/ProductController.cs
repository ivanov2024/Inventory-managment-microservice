using InventoryManagment.Models;
using InventoryManagment.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagment.Controllers
{
    public class ProductController : Controller
    {
        const string _dateFormat = "dddd-MMMM-yyyy";

        private readonly IProductService _productService;
        public ProductController(IProductService productService)
            => _productService = productService;

        private static string FormatDate(DateTime? date)
            => date?.ToString(_dateFormat)!;

        public async Task<IActionResult> Index()
        {
            var products
                = await _productService
                .GetAllProductsAsync();

            var productsToViewModel
                = products
                .Select(product => new ProductViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = product.Quantity,
                    Description = product.Description,
                    CreatedAt = FormatDate(product.CreatedAt),
                    UpdatedAt = FormatDate(product.UpdatedAt),
                    CategoryId = product.CategoryId,
                })
                .ToList();

            return View(productsToViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetProductById(int productId)
        {
            var product
                = await _productService
                .GetProductByIdAsync(productId);

            ProductViewModel productViewModel
                = new()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = product.Quantity,
                    Description = product.Description,
                    CreatedAt = FormatDate(product.CreatedAt),
                    UpdatedAt = FormatDate(product.UpdatedAt),
                    CategoryId = product.CategoryId,
                };

            return View(productViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductViewModel productViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(productViewModel);
            }

            var createdProduct
                = await _productService
                .CreateProductAsync(productViewModel);

            if (!createdProduct)
            {
                ModelState
                    .AddModelError(string.Empty, "Error creating a product");

                return View(productViewModel);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}