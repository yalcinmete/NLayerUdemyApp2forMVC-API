using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;

namespace NLayer.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _services;
        private readonly ICategoryService _categoryservices;
        private readonly IMapper _mapper;

        public ProductsController(IProductService services, ICategoryService categoryservices, IMapper mapper)
        {
            _services = services;
            _categoryservices = categoryservices;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            //GetProductsWithCategory()'den CustomResponseDto döndüğü için ve bizim işimizi görmediği için CustomResponse yapıp datayı view'e gönderdik.
            //    var CustomResponse = await _services.GetProductsWithCategory();

            //    return View(CustomResponse.Data);
            //}

            //Yukarıdaki işlemden de vazgeçtik GetProductsWithCategory()'den CustomResponseDto dönmesini gerekli yerlerden kaldırdık.GetProductsWithCategory()'dan artık direk ProductWithcategory dönüyor.

            return View(await _services.GetProductsWithCategory());
        }

        //MVC Video47
        public async Task<IActionResult> Save()
        {
            var categories = await _categoryservices.GetAllAsync();
            //categories IEnumrable dönüyor bu nedenle categories.ToList() ile list'e çevirdik.
            var categoriesDto = _mapper.Map<List<CategoryDto>>(categories.ToList());

            //categoriesDto'dan ıd leri al nameleri selectlistte göster.
            ViewBag.categories = new SelectList(categoriesDto, "Id", "Name");

            return View();
        }

        //MVC Video47.
        [HttpPost]
        public async Task<IActionResult> Save(ProductDto productDto)
        {

            if (ModelState.IsValid)
            {
                //hata yoksa;
                await _services.AddAsync(_mapper.Map<Product>(productDto));
                //return RedirectToAction("Index"); de diyebiliriz ama tip güvenliği için;
                return RedirectToAction(nameof(Index));
            }


            var categories = await _categoryservices.GetAllAsync();

            //categories IEnumrable dönüyor bu nedenle categories.ToList() ile list'e çevirdik.
            var categoriesDto = _mapper.Map<List<CategoryDto>>(categories.ToList());

            //categoriesDto'dan ıd leri al nameleri selectlistte göster.
            //Post işleminde hata var ise (Hata yoksa zaten redirect ile biz başka sayfaya yönlendirme yapacağız.) bu sayfa yeniden çalışır bu aşamada da selectlist'in dolu olması lazım.Yoksa dropdown boş olur.O yuzden Gettekilerin aynısını postta da yazdık.
            ViewBag.categories = new SelectList(categoriesDto, "Id", "Name");

            //Başarısız ise, aynı sayfayı tekrar dönsün.
            return View();
        }

        //MVC Video53. NotfoundFilter ctor'da parametre geçtiği için ServiceFilter ile eklenmelidir.
        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        //Video 50. ProductController Update-1.
        public async Task<IActionResult> Update(int id)
        {
            var product = await _services.GetByIdAsync(id);

            var categories = await _categoryservices.GetAllAsync();
            //categories IEnumrable dönüyor bu nedenle categories.ToList() ile list'e çevirdik.
            var categoriesDto = _mapper.Map<List<CategoryDto>>(categories.ToList());

            //categoriesDto'dan ıd leri al nameleri selectlistte göster. Son parametresi seçilen değer.
            ViewBag.categories = new SelectList(categoriesDto, "Id", "Name",product.CategoryId);

            return View(_mapper.Map<ProductDto>(product));

        }

        //Video 50. ProductController Update-1.
        [HttpPost]
        public async Task<IActionResult> Update(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                await _services.UpdateAsync(_mapper.Map<Product>(productDto));
                return RedirectToAction(nameof(Index));
            }

            //Kullanıcı hata ile karşılaşırsa burası çalışacağı için selectlistler tekrar dolsun.

            var categories = await _categoryservices.GetAllAsync();
            //categories IEnumrable dönüyor bu nedenle categories.ToList() ile list'e çevirdik.
            var categoriesDto = _mapper.Map<List<CategoryDto>>(categories.ToList());

            //categoriesDto'dan ıd leri al nameleri selectlistte göster. Son parametresi seçilen değer.
            ViewBag.categories = new SelectList(categoriesDto, "Id", "Name", productDto.CategoryId);

            return View(productDto);
        }

        public async Task<IActionResult> Remove(int id)
        {
            var product = await _services.GetByIdAsync(id);

            await _services.RemoveAsync(product);
            return RedirectToAction(nameof(Index));
        }
    }
}
