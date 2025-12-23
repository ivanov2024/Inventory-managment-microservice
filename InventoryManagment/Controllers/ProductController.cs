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
        
        public async Task<IActionResult> Index()
        {
            var products 
                = await _productService
                .GetAllProductsAsync();

            return View(products);
        }

        public async Task<IActionResult> GetProductById(int productId)
        {
            var product
                = await _productService
                .GetProductById(productId);

            ProductViewModel productViewModel
                = new()
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

            return View(productViewModel);
        }
    }
}
