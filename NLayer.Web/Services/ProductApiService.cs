using NLayer.Core.DTOs;
using System.Net.Http;

namespace NLayer.Web.Services
{
    //Video 56 MVC-API haberleşmesi
    //Artık istediğim sınıfın constructorunda ProductApiService geçip direk kullanabilirim. HttpClient'i new()leyerek kullanmak BestPractice değildir. DI container'a bu işi bırakın.(Program.cs'de gerekli yapılandırmasından bahsediyoruz.Daha performanslı ve daha az nesne örneği üreterek httpclienti kullanabiliriz.Bu sayede bir socket yokluğu gibi problemlerle karşılaşmayız.
    public class ProductApiService
    {
        private readonly HttpClient _httpClient;

        public ProductApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        //API ProductController'da veriyi service(IProductService)'den çekiyorduk. Oraya gitmeye çalışıyoruz :
        public async Task<List<ProductWithCategoryDto>> GetProductsWithCategoryAsync()
        {
            //Daha önceden 
            //var response2 = await _httpClient.GetAsync("products/GetProducrsWithCategory"); şeklinde yaptıktan sonra istek başarılı ise response2.Content.ReadAsStringAsync() ile okuyorduk.Okuduktan sonra da tabijsona cast ediyorduk. Artık buna gerek yok. Aşağıdaki gibi direkt json veren işlemi yapabiliriz.

            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<List<ProductWithCategoryDto>>>("product/GetProductsWithCategory");

            return response.Data;
    
        }

        //Save için yapalım şimdi de ;
        public async Task<ProductDto> SaveAsync(ProductDto newProduct)
        {
            //var response = await _httpClient.PostAsync yapardık ve gelen datayı jsona çeviriyorduk.

            //Artık json olarak data gönder diyebiliyoruz : products endpointine gidecek ve / diye ekleme yapmamıza gerek yok çünkü API tarafın post işlemi yaptığımız zaman [HttpPost] attiribute'ne sahip SAVE metodu çalışacaktır. base url'yi program cs'den eklemiştik.https://localhost:7297/api/product ve post metodu calısmıs olucak. product objesini de yine veriyoruz.
            var response = await _httpClient.PostAsJsonAsync("product", newProduct);

            //kayıt yapılmamış ise null dön. ya da burada loglama da yapabilirsin.
            if (!response.IsSuccessStatusCode) return null;

            //başarılı ise bodysini okumam lazım.
            //var responseBody = await response.Content.ReadAsStringAsync(); //contentini asenkron olarak okuyor.Buradan string geliyor. .net 5.0 ile daha güzel bir yolu var .ReadFromJsonAsync.

            //CustomResponseDto olarak oku. İçinde ProductDto olmasını bekliyorum.bu şekilde json olarak oku.
            var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<ProductDto>>();

            return responseBody.Data;
        }


        //GetById için ;
        public async Task<ProductDto> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<ProductDto>>($"product/{id}");

            ////hata oluşmuş ise
            //if (response.Errors.Any())
            //{
            //    //log tutabilirsin.
            //}

            return response.Data;
        }


        //Update için yapalım şimdi de ;
        public async Task<bool> UpdateAsync(ProductDto newProduct)
        {
            //Yukarıdaki save'i kopyalayıp yapıstırdık üzerinde düzenleme yaptık.
            var response = await _httpClient.PutAsJsonAsync("product", newProduct);

            return response.IsSuccessStatusCode;
        }


        //Remove için yapalım şimdi de ;
        public async Task<bool> RemoveAsync(int id)
        {
            //Yukarıdaki save'i kopyalayıp yapıstırdık üzerinde düzenleme yaptık.
            var response = await _httpClient.DeleteAsync($"product/{id}");

            return response.IsSuccessStatusCode;
        }




    }
}
