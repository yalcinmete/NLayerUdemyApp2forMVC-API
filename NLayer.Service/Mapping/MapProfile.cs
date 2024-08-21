using AutoMapper;
using NLayer.Core.DTOs;
using NLayer.Core.Models;

namespace NLayer.Service.Mapping
{
    //Video29. ReverseMap() yaparsak ProductDtoları da Producta maple demek olur.
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<ProductFeature, ProductFeatureDto>().ReverseMap();
            CreateMap<ProductUpdateDto, Product>();

            //Video33 sonu. Productsı ProductWithCategoryDto'ya çeviriyoruz.
            CreateMap<Product, ProductWithCategoryDto>();

            //Video34.
            CreateMap<Category, CategoryWithProductDto>();
        }
    }
}
