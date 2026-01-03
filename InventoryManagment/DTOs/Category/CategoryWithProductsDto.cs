using InventoryManagment.DTOs.Product;
using System.Text.Json.Serialization;

namespace InventoryManagment.DTOs.Category
{
    public class CategoryWithProductsDto : CategoryDto
    {
        [JsonPropertyOrder(4)]
        public List<ProductDto> Products { get; set; } 
            = new();
    }
}
