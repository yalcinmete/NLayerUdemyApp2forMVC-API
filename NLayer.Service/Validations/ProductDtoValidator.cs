using FluentValidation;
using NLayer.Core.DTOs;

namespace NLayer.Service.Validations
{
    public class ProductDtoValidator : AbstractValidator<ProductDto>
    {
        public ProductDtoValidator()
        {
            RuleFor(x => x.Name).NotNull().WithMessage("{PropertyName} is required").NotEmpty().WithMessage("{PropertyName} is required"); //NotNull() NotEmpy() hem boş olmasın hem null olmasın.

            //Price(decimal),Stock(int) kullanıcı bu değerleri boş göndermiş olsa bile default olarak 0(sıfır) atanır.Bu nedenle NotEmpty() NotNull() burada kullanılmaz.Bu nedenle 0'dan farklı olması icin Aralık verebiliriz.CategoryId'nin de default değeri 0'dır
            RuleFor(x => x.Price).InclusiveBetween(1, int.MaxValue).WithMessage("{PropertyName} must be greater 0");
            RuleFor(x => x.Stock).InclusiveBetween(1, int.MaxValue).WithMessage("{PropertyName} must be greater 0");

            //MVC Vİdeo49. Dropdown ile bunu dolduruyoruz.CategoryId'yı kontrol etmeye gerek yok.
            //RuleFor(x => x.CategoryId).InclusiveBetween(1, int.MaxValue).WithMessage("{PropertyName} must be greater 0");
        }
    }
}
