using System.ComponentModel.DataAnnotations;

namespace InventoryManagment.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(150)]
        public string Description { get; set; } = null!;

        [Required]
        public string CreatedAt { get; set; } = null!;

        public string? UpdatedAt { get; set; }

        public int CategoryId { get; set; }
    }
}
