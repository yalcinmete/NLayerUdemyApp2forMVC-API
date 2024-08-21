using NLayer.Core.DTOs;
using NLayer.Core.Models;

namespace NLayer.Core.Services
{
    public interface IProductService : IService<Product>
    {
        //Task<List<ProductWithCategoryDto>> GetProductsWithCategory();

        //API controllerdeki actionlar da hep customResponseDto çevirme işlemi yapıyorduk.Bu işlemi controllerApı'da değil serviste yapmış olalım.
        //Task<CustomResponseDto<List<ProductWithCategoryDto>>> GetProductsWithCategory();

        //MVC Video45.MVC projesinde CustomResponseDto ihtiyacımız yok.API'ya ekle ama.
        Task<List<ProductWithCategoryDto>> GetProductsWithCategory();
    }
}
