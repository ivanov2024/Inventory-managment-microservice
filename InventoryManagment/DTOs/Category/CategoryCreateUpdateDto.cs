using System.ComponentModel.DataAnnotations;

namespace InventoryManagment.DTOs.Category
{
    public class CategoryCreateUpdateDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        [Required]
        [MinLength(3)]
        [MaxLength(150)]
        public string Description { get; set; } = null!;
    }
}
