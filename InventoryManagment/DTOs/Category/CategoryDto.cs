using System.Text.Json.Serialization;

namespace InventoryManagment.DTOs.Category
{
    public class CategoryDto
    {
        [JsonPropertyOrder(1)]
        public int Id { get; set; }

        [JsonPropertyOrder(2)]
        public string Name { get; set; } = null!;

        [JsonPropertyOrder(3)]
        public string Description { get; set; } = null!;
    }
}
