namespace NLayer.Core.DTOs
{
    //Video33. Productlarla birlikte categoryleri de döndürme işlemine gittiğimiz için, servis tarafında özelleştirilmiş ProductWithCategoryDto dönelim.
    //Repositoryler entity dönerken, servisler dto dönmeli.API'nın isteyeceği DTOyu dönmeli.API'da bir daha bir şeyleri değiştirmekle uğraşmıyoruz.
    public class ProductWithCategoryDto : ProductDto
    {
        public CategoryDto Category { get; set; }
    }
}
