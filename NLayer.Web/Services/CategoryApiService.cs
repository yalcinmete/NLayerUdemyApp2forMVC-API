using NLayer.Core.DTOs;

namespace NLayer.Web.Services
{
    ////Video 56 MVC-API haberleşmesi
    //Artık istediğim sınıfın constructorunda CategoryApiService geçip direk kullanabilirim. HttpClient'i new()leyerek kullanmak BestPractice değildir. DI container'a bu işi bırakın .(Program.cs'de gerekli yapılandırmasından bahsediyoruz.)Daha performanslı ve daha az nesne örneği üreterek httpclienti kullanabiliriz.Bu sayede bir socket yokluğu gibi problemlerle karşılaşmayız.
    public class CategoryApiService
    {
        private readonly HttpClient _httpClient;

        public CategoryApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CategoryDto>> GetAllAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<List<CategoryDto>>>("categories");
            return response.Data;
        }
    }
}
