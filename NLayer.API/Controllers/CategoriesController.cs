using Microsoft.AspNetCore.Mvc;
using NLayer.API.Filters;
using NLayer.Core.Repositories;
using NLayer.Core.Services;

namespace NLayer.API.Controllers
{
    [ValidateFilterAttribute] //Video37.Oluşturmuş olduğumuz FilterAttribute bu şekilde veriyoruz.Controlin başına koyunca tüm actionlarda geçerli olur ama tüm controllerde (Product,Categories...) tek tek eklemek yerine program.cs'e gidip bunu da temiz bir şekilde yapabiliriz.
    public class CategoriesController : CustomBaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // api/categories/GetSingleCategoryByIdWithProducts/2
        [HttpGet("[action]/{categoryId}")]
        public async Task<IActionResult> GetSingleCategoryByIdWithProducts(int categoryId)
        {
            return CreateActionResult(await _categoryService.GetSingleCategoryByIdWithProductsAsync(categoryId));
        }
    }
}
