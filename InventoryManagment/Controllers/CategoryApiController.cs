using InventoryManagment.Data.Models;
using InventoryManagment.Models;
using InventoryManagment.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryApiController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryApiController(ICategoryService categoryService)
            => _categoryService = categoryService;   

        [HttpGet]
        public async Task<IEnumerable<Category>> GetAll()
            => await _categoryService.GetAllCategoriesAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> Get(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);

            if (category == null) return NotFound();

            return category;
        }

        [HttpPost]
        public async Task<ActionResult<Category>> Create([FromBody] CategoryViewModel categoryViewModel)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);
            
            var createdCategory = await _categoryService.CreateCategoryAsync(categoryViewModel);

            // Returns 201 Created with a Location header pointing to the newly created resource
            return CreatedAtAction(nameof(Get), new {id = createdCategory.Id} ,createdCategory);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryViewModel categoryViewModel)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            var isUpdated = await _categoryService.UpdateCategoryAsync(categoryViewModel, id);

            if (!isUpdated) return NotFound();

            //Faster update without returning the updated entity
            return NoContent();

            // Alternatively, return the updated entity if the update method returns the updated entity
            // and details are needed
            // return OK(updatedCategory);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await _categoryService.DeleteCategoryAsync(id);

            if (!isDeleted) return NotFound();

            //Better to return NoContent for delete operations
            return NoContent();

            // Alternatively, return OK with a message
            // return OK(new { message = "Category deleted successfully." });
        }
    }
}
