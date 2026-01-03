using InventoryManagment.Data.Models;
using InventoryManagment.Models;
using InventoryManagment.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsApiController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsApiController(IProductService productService)
            => _productService = productService;   

        [HttpGet]
        public async Task<IEnumerable<Product>> GetAll()
            => await _productService.GetAllProductsAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> Get(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null) return NotFound();

            return product;
        }
    }
}
