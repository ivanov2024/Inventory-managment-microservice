using System.ComponentModel.DataAnnotations;

namespace InventoryManagment.DTOs.Product
{
    public class ProductCreateUpdateDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(150)]
        public string Description { get; set; } = null!;

        [Required]
        public int CategoryId { get; set; }
    }
}
