using InventoryManagment.DTOs.StockTransaction;
using InventoryManagment.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagment.Controllers
{
    [ApiController]
    [Route("api/products/{productId}/stock")]
    public class StockController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public StockController(IInventoryService inventoryService)
            => _inventoryService = inventoryService;

        [HttpPost("increase")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> IncreaseStock(int productId, [FromBody] StockChangeRequestDto dto)
        {
            await _inventoryService.IncreaseStockAsync(productId, dto);
            // 204 = success, nothing to return
            // could return 200 with updated stock level if needed
            return NoContent();
        }

        [HttpPost("decrease")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DecreaseStock(int productId, [FromBody] StockChangeRequestDto dto)
        {
            await _inventoryService.DecreaseStockAsync(productId, dto);
            // 204 = success, nothing to return
            // could return 200 with updated stock level if needed
            return NoContent();
        }

        [HttpGet("transactions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTransactions(int productId)
        {
            var transactions = await _inventoryService.GetTransactionsForProductAsync(productId);
            return Ok(transactions);
        }
    }

}
