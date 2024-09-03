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
        //Task<List<ProductWithCategoryDto>> GetProductsWithCategory();

       //Video55 MVC-API haberlesmesi.API 'yı kapatmıstık MVC ye göre GetProductsWithCategory()dönüs tipindeki CustomResponseDto dönmesin demiştik. İlk bunu eski haline cevirmekle baslıyoruz.Çünkü API' artık ayakta olucak.
        Task<CustomResponseDto<List<ProductWithCategoryDto>>> GetProductsWithCategory();
    }
}
