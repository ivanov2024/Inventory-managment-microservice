using System.ComponentModel.DataAnnotations;

namespace InventoryManagment.Data.Models
{
    public class StockTransaction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public virtual Product Product { get; set; } = null!;

        [Required]
        public int ChangeAmount { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(150)]
        public string Reason { get; set; } = null!; 

        public DateTime CreatedAt { get; set; }
    }
}
