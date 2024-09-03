using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NLayer.API.Filters;
using NLayer.Core.DTOs;
using NLayer.Core.Repositories;
using NLayer.Core.Services;

namespace NLayer.API.Controllers
{
    //[ValidateFilterAttribute] //Video37.Oluşturmuş olduğumuz FilterAttribute bu şekilde veriyoruz.Controlin başına koyunca tüm actionlarda geçerli olur ama tüm controllerde (Product,Categories...) tek tek eklemek yerine program.cs'e gidip bunu da temiz bir şekilde yapabiliriz.
    public class CategoriesController : CustomBaseController
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        //Video58.MVC'deki category select listi doldurabilmek için Category'in hepsini çektiğimiz bir metodumuz olmalı.
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();
            
            //categories.Tolist() 'i categoryDto List'e çevir. 
            var categoriesDto = _mapper.Map<List<CategoryDto>>(categories.ToList());

            return CreateActionResult(CustomResponseDto<List<CategoryDto>>.Success(200, categoriesDto));
            
        }


        // api/categories/GetSingleCategoryByIdWithProducts/2
        [HttpGet("[action]/{categoryId}")]
        public async Task<IActionResult> GetSingleCategoryByIdWithProducts(int categoryId)
        {
            return CreateActionResult(await _categoryService.GetSingleCategoryByIdWithProductsAsync(categoryId));
        }
    }
}
