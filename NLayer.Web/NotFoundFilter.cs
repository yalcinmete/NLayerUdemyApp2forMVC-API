using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;

namespace NLayer.Web
{
    public class NotFoundFilter<T> : IAsyncActionFilter where T : BaseEntity
    {
        private readonly IService<T> _service;

        //MVC Video 53.Filterinin contstructorunda DI geçiyorsan program.cs'e bunu eklemelisin.
        public NotFoundFilter(IService<T> service)
        {
            _service = service;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            //Controller actiondaki ilk parametreyi (int id) alıyoruz.
            var idValue = context.ActionArguments.Values.FirstOrDefault();
            if (idValue == null)
            {
                //id null ise yoluna devam et diyoruz.
                await next.Invoke();
                return;
            }
            var id = (int)idValue;
            //var anyEntity = await _service.GetByIdAsync(id);
            var anyEntity = await _service.AnyAsync(x => x.Id == id); //x.Id'yi yakalayabilmek için baseEntity verdik yukarıda.

            //Eğer id'ye ait entity var ise ;
            if (anyEntity)
            {
                await next.Invoke();
                return;
            }

            ////MVC Video53. API tarafında NotFoundObjectResult dönderiyorduk. MVC tarafında  notfound error sayfasına yönlendireceğiz.
            ////Data yok ise; 
            //context.Result = new NotFoundObjectResult(CustomResponseDto<NoContentDto>.Fail(404, $"{typeof(T).Name}({id}) not found"));

            var errorViewModel = new ErrorViewModel();
            errorViewModel.Errors.Add($"{typeof(T).Name}({id}) not found");
            context.Result = new RedirectToActionResult("Error", "Home", errorViewModel);
        }
    }
}
