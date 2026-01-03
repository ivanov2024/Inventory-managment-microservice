using AutoMapper;
using InventoryManagment.Data.Models;
using InventoryManagment.DTOs.Category;
using InventoryManagment.DTOs.Product;

namespace InventoryManagment.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryDto>();

            CreateMap<Category, CategoryWithProductsDto>();

            CreateMap<Product, ProductDto>();
        }
    }
}
