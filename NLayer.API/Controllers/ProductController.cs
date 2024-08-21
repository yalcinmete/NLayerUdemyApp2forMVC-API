using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLayer.API.Filters;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;

namespace NLayer.API.Controllers
{

    //başındaki bu attributeler miras alınan classda olduğu için (CustomBaseController) bir daha buraya eklemeye gerek yok.
    //[Route("api/[controller]")] 
    //[ApiController]

    //public class ProductController : ControllerBase
    public class ProductController : CustomBaseController //Dönüş değerlerimizi düzenlediğimiz custombase controller oluşturduk.Bu classtan miras aldık. 
    {
        private readonly IMapper _mapper;
        //private readonly IService<Product> _service; //Video35.productService oluşturduğumuz için artık bu servisle çalışmayacağız.IProductService ile devam.
        //private readonly IProductService productService;

        private readonly IProductService _service;

        public ProductController(IMapper mapper, IProductService productService)
        {
            _mapper = mapper;
            //_service = service;
            _service = productService;
        }

        // GET api/products/GetProductsWithCategory çağrılır.
        //Birden fazla HttpGet olduğu için EF hangisini çağıracağını bilemez. Bu nedenle bu yeni oluşturduğumuz ikinci geti HttpGet("GetProductsWithCategory") şeklinde tanımlıyoruz.
        //[HttpGet("GetProductsWithCategory")] yerine  [HttpGet("[action]")] da kullanabilirsin.
        [HttpGet("GetProductsWithCategory")]
        public async Task<IActionResult> GetProductsWithCategory() 
        {
            //CreateActionResult'ın istediği tipi(CustomResponseDto) serviste oluşturmuş olduk(productService).Burası daha yalın oldu. yani API'nın istediği dönüş tipini servislerde oluşturmak daha mantıklı.
            //return CreateActionResult(await productService.GetProductsWithCategory()); //Video35.productService yerine _service kullanıyoruz.
            return CreateActionResult(await _service.GetProductsWithCategory());
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var products = await _service.GetAllAsync();

            //productsları verdim _mapper productDtoya çevirecek.
            var productsDtos = _mapper.Map<List<ProductDto>>(products.ToList()); //productslar IEnumerable dönüyor.Tolist yardımıyla listeye çevirdik.

            //return Ok(CustomResponseDto<List<ProductDto>>.Success(200,productsDtos)); 
            //bu şekilde dönüş sayfada kirliliğe neden olduğu için bir tane basecontroller oluşturup dönüş işlemlerini orada yapalım(CustomBaseController)

            return CreateActionResult(CustomResponseDto<List<ProductDto>>.Success(200, productsDtos));
        }


        //Video40sonu.[ValidateFilter] gibi [NotFoundFilteri] kullanamayız.Çünkü NotFoundFilter Attribute classını imlemente etmiyor ve constructorunda parametre geçiyorsun(DI).Bu nedenle [ServiceFilter] ile kullanmak zorundayız.
        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        //[HttpGet("id")] id burada vermezsen metot parametresindeki id'yi querystringten bekler(www.mysite.com/api/products?id=5).
        //Id belirtirsek www.mysite.com/api/products/5  5 olan data gelir.
        //Eğer biden çok id dönmek istersen www.mysite.com/api/products/5/3  [HttpGet("{id},{id2}")]
        //Ayrıca metodun tipine(HttpGet) ve parametresine göre eşleşme var.
        //Ayrıca [Route("api/[controller]/[action]")] şeklinde verirsen bu sefer action ismini de url'de belirtmen gerekirdi.
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _service.GetByIdAsync(id);

            //Video39.null kontrolünü burada yapabiliriz ama her action içine tek tek null kontrolü yapmak zorunda kalıyoruz bu istemediğimiz bir durum. Bu kontrolü servis katmanında servis classında yapmalıyız. 
            //if (product==null)
            //{
            //    return CreateActionResult(CustomResponseDto<ProductDto>.Fail(400, "Bu id'ye sahip ürün bulunamadı"));
            //}

            var productsDto = _mapper.Map<ProductDto>(product);
            return CreateActionResult(CustomResponseDto<ProductDto>.Success(200, productsDto));
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProductDto productDto)
        {
            var product = await _service.AddAsync(_mapper.Map<Product>(productDto));
            var productsDto = _mapper.Map<ProductDto>(product);
            return CreateActionResult(CustomResponseDto<ProductDto>.Success(201, productsDto));
        }

        [HttpPut]
        public async Task<IActionResult> Update(ProductUpdateDto productDto)
        {
            await _service.UpdateAsync(_mapper.Map<Product>(productDto));
            //Bir nesne dönmeyeceğimiz için oluşturduğumuz NoContentDto'yu burada kullanıyoruz.
            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }

        //DELETE api/products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var product = await _service.GetByIdAsync(id);


            //Normalde bu kontrolü yapmak lazım ama bunu ileriki derslerimizde başka bir yerden yapacağız.
            //if (product == null)
            //{
            //    return CreateActionResult(CustomResponseDto<NoContentDto>.Fail(404,"bu id'ye sahip ürün bulunamadı"));
            //}

            await _service.RemoveAsync(product);
            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }
    }
}
