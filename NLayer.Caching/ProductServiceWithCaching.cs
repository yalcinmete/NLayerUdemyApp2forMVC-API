using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Service.Exceptions;
using System.Linq.Expressions;

namespace NLayer.Caching
{
    //Video42 Caching Apı videoların sonu.
    public class ProductServiceWithCaching : IProductService
    {
        private const string CacheProductKey = "productCache";
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private readonly IProductRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductServiceWithCaching(IMapper mapper, IMemoryCache memoryCache, IProductRepository repository, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _memoryCache = memoryCache;
            _repository = repository;
            _unitOfWork = unitOfWork;

            //bir metotda birden çok değer dönmek istiyorsak out kullanılır.Biraz daha best practice'si tuple kullanılır.
            //_ kullanarak boşuna bellekte allocate etmesin cachedeki datayı almak istemiyorum  Sadece cacheProductKey'ine ait data var mı yok mu onu kontrol etmek istiyorum.
            if (!_memoryCache.TryGetValue(CacheProductKey, out _))
            {
                //Eğer cacheProductKey yoksa uygulama ilk ayağa kalktığında constructorda oluşturacak.
                //Constructor içinde asenkron metot kullanamazsın ek bilgi.
                _memoryCache.Set(CacheProductKey, _repository.GetAll().ToList());

                //Productlarla birlikte kategorileri de çekmek istersek ama bu sefer bu sayfadaki GetAllAsync'nin bir anlamı kalmıyor.Çünkü GetAllAsync de buraya göre veri çektiği için burada ne çekersen GetAllAsync'de onu çekeceğin için GetAllAsync'nin bir faydası kalmıyor.GetAllAsync 'nin de yine sayfadaki GetProductsWithCategory() İkisininde içinde kategori oluyor. Ama ProductController'da api/products All() metoduna geldiğinde istek productDto dönüştürme işlemi olduğu için ve ProductDto'da category olmadığı için category ekrana basılmayacak.
                //Video44 sonu.Constructorın içinde asenkron metot kullanamıyoruz bu nedenle GetProductsWithCategory'i result ile senkrona çeviriyoruz.
                _memoryCache.Set(CacheProductKey, _repository.GetProductsWithCategory().Result);
            }
        }



        public async Task<Product> AddAsync(Product entity)
        {
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            await CacheAllProductsAsync();
            return entity;
        }

        public async Task<IEnumerable<Product>> AddRangeAsync(IEnumerable<Product> entities)
        {
            await _repository.AddRangeAsync(entities);
            await _unitOfWork.CommitAsync();
            await CacheAllProductsAsync();
            return entities;
        }

        public Task<bool> AnyAsync(Expression<Func<Product, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetAllAsync()
        {
            var products = _memoryCache.Get<IEnumerable<Product>>(CacheProductKey);
            return Task.FromResult(products);
        }

        public Task<Product> GetByIdAsync(int id)
        {


            var product = _memoryCache.Get<List<Product>>(CacheProductKey).FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                throw new NotFoundException($"{typeof(Product).Name}({id}) not found");
            }
            //veriyi cache'den aldığımız için asenkron bir işlem yapmadığımız için yukarıda bir await kullanmadığımız için await kullanmadığımız için bir async de koymak zorunda değiliz ama bizden bir Task<Product> bekliyor . Bu nedenle Task.FromResult kullanmak zorunda kaldık.
            return Task.FromResult(product);
        }


        //MVC Video45.MVC projelerinde Success,Fail dönmemize gerek yok.CustomResponseDto dönmemize de gerek yok.
        //public Task<CustomResponseDto<List<ProductWithCategoryDto>>> GetProductsWithCategory()
        public Task<List<ProductWithCategoryDto>> GetProductsWithCategory()
        {
            //Video43 sonları.GetProductsWithCategory metodu Productları categorylarle beraber çekme işlemi nadir olarak kullanılıyrosa cache yerine repo'dan çekebiliriz.
            //var producs = await _repository.GetProductsWithCategory();

            //Degiştirdik.
            var products = _memoryCache.Get<IEnumerable<Product>>(CacheProductKey);
            var productsWithCategoryDto = _mapper.Map<List<ProductWithCategoryDto>>(products);

            return Task.FromResult(productsWithCategoryDto);
        }

        public async Task RemoveAsync(Product entity)
        {
            _repository.Remove(entity);
            await _unitOfWork.CommitAsync();
            //silinme işleminden sonra cache'i yeniden çağırmamız lazım tabi.
            await CacheAllProductsAsync();
        }

        public async Task RemoveRangeAsync(IEnumerable<Product> entities)
        {
            _repository.RemoveRange(entities);
            await _unitOfWork.CommitAsync();
            await CacheAllProductsAsync();
        }

        //Vİde43.Cacheleme işlemini fonksiyona atadık.
        public async Task UpdateAsync(Product entity)
        {
            _repository.Update(entity);
            await _unitOfWork.CommitAsync();
            await CacheAllProductsAsync();
        }

        public IQueryable<Product> Where(Expression<Func<Product, bool>> expression)
        {
            //Artık Cache'de sorgulama yapmam lazım.
            //lİNQ Sorgusunu EF'den çekmiyorum cacheden çektiğim için expression.Compile eklemem gerekiyor.
            return _memoryCache.Get<List<Product>>(CacheProductKey).Where(expression.Compile()).AsQueryable();
        }

        public async Task CacheAllProductsAsync()
        {
            _memoryCache.Set(CacheProductKey, await _repository.GetAll().ToListAsync());
        }
    }
}
