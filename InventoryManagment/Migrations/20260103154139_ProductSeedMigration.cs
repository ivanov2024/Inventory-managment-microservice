using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InventoryManagment.Migrations
{
    /// <inheritdoc />
    public partial class ProductSeedMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Description", "Name", "Price" },
                values: new object[,]
                {
                    { 1, 1, "Latest Apple smartphone with A17 chip, 6.7-inch display, and advanced camera system.", "iPhone 15", 1199.99m },
                    { 2, 1, "Flagship Samsung phone with cutting-edge features and high-resolution AMOLED display.", "Samsung Galaxy S24", 1099.99m },
                    { 3, 2, "Comprehensive guide to C# programming for beginners and professionals.", "C# Programming Book", 49.99m },
                    { 4, 3, "Comfortable cotton t-shirt, available in multiple colors and sizes.", "Men's T-Shirt", 19.99m },
                    { 5, 4, "High-power kitchen blender perfect for smoothies, soups, and sauces.", "Blender", 89.99m },
                    { 6, 4, "Automatic coffee maker with programmable timer and multiple brewing options.", "Coffee Maker", 129.99m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}
