using System.Text.Json.Serialization;

namespace NLayer.Core.DTOs
{
    //Video30.Endpointlerde yapılan işlemler sonucunda geri dönüş dto'sudur. Fail veya başarılı ise dataları döneriz.
    public class CustomResponseDto<T>
    {

        public T Data { get; set; }

        //Vide30.StatusCode'u cliente göstermek istemiyoruz (çünkü zaten clientler istek yapınca statuscode otomatik görüyor) ama kod içinde kullanacağız. Bu nedenle dönüş olan jsona eklememek için [JsonIgnore] kullanıyoruz
        [JsonIgnore]
        public int StatusCode { get; set; }

        public List<String> Errors { get; set; }

        //Hangi sınıfı dönmek istiyorsan o sınıf içerisinde static metotlar tanımlayarak geriye instancelar dönmek yani new anahtar sözcüğünü kullanmak yerine bu metotları kullanmak nesne üretmek olayı. Buna staticfactory denir.Böylece nesne oluşturmayı kontrol altına almış olursun. DesignPatternlerin amacı zaten nesne üretme olayını soyutlamak,nesne üretme olayını clientlardan almak, tamamen ayrı bir yerden oluşturmak.
        public static CustomResponseDto<T> Success(int statusCode, T data)
        {
            return new CustomResponseDto<T> { Data = data, StatusCode = statusCode };
        }

        public static CustomResponseDto<T> Success(int statusCode)
        {
            return new CustomResponseDto<T> { StatusCode = statusCode };
        }

        public static CustomResponseDto<T> Fail(int statuscode, List<string> errors)
        {
            return new CustomResponseDto<T> { StatusCode = statuscode, Errors = errors };
        }

        //Bir hata da dönebiliriz ve liste tanımı olduğu için tek listede bir hata da gösterebiliriz.Bir daha yukarıda string hata tanımlamaya gerek yok.
        public static CustomResponseDto<T> Fail(int statuscode, string errors)
        {
            return new CustomResponseDto<T> { StatusCode = statuscode, Errors = new List<string> { errors } };
        }
    }
}
