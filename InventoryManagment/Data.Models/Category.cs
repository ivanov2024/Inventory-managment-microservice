using System.ComponentModel.DataAnnotations;

namespace InventoryManagment.Data.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        [Required]
        [MinLength(3)]
        [MaxLength(150)]
        public string Description { get; set; } = null!;

        public virtual ICollection<Product> Products { get; set; } 
            = new HashSet<Product>();
    }
}
