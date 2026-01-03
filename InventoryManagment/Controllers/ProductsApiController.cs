using InventoryManagment.DTOs.Product;
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
        public async Task<IEnumerable<ProductDto>> GetAll()
            => await _productService.GetAllProductsAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> Get(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null) return NotFound();

            return product;
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> Create([FromBody] ProductCreateUpdateDto productDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var createdProduct = await _productService.CreateProductAsync(productDto);

            // Returns 201 Created with a Location header pointing to the newly created resource
            return CreatedAtAction(nameof(Get), new { id = createdProduct.Id }, createdProduct);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductCreateUpdateDto productDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var isUpdated = await _productService.UpdateProductAsync(productDto, id);

            if (!isUpdated) return NotFound();

            //Faster update without returning the updated entity
            return NoContent();

            // Alternatively, return the updated entity if the update method returns the updated entity
            // and details are needed
            // return OK(updatedProduct);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await _productService.DeleteProductAsync(id);

            if (!isDeleted) return NotFound();

            //Better to return NoContent for delete operations
            return NoContent();

            // Alternatively, return OK with a message
            // return OK(new { message = "Product deleted successfully." });
        }
    }
}
