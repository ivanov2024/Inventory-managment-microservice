using AutoMapper;
using InventoryManagment.Data.Models;
using InventoryManagment.DTOs.Category;
using InventoryManagment.DTOs.Product;
using InventoryManagment.DTOs.StockTransaction;

namespace InventoryManagment.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryDto>();
            CreateMap<Category, CategoryWithProductsDto>();
            CreateMap<CategoryCreateUpdateDto, Category>();

            CreateMap<Product, ProductDto>();
            CreateMap<ProductCreateUpdateDto, Product>();

            CreateMap<StockTransaction, StockTransactionDto>();
            CreateMap<StockTransactionDto, StockTransaction>();
        }
    }
}
