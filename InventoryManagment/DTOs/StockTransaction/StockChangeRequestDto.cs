using System.ComponentModel.DataAnnotations;

namespace InventoryManagment.DTOs.StockTransaction
{
    /// <summary>
    /// Input DTO for creating or updating stock change requests.
    /// </summary>
    public class StockChangeRequestDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public int Amount { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(150)]
        public string Reason { get; set; } = null!;
    }
}
