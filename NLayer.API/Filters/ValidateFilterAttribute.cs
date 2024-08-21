using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLayer.Core.DTOs;

namespace NLayer.API.Filters
{
    //Video37 FluentValidation
    public class ValidateFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //Valitate işlemlerin ModelState ile entegre mapleniyor.
            if (!context.ModelState.IsValid) 
            {
                var errors = context.ModelState.Values.SelectMany(x=>x.Errors).Select(x=>x.ErrorMessage).ToList();

                //BadRequestObjectResult  ; Response'ın bodysinde hata mesajlarını göndermiş oluyoruz.
                //BadRequestResult  ; Response'ın bodysinde hata mesajları göndermiyoruz.
                context.Result = new BadRequestObjectResult(CustomResponseDto<NoContentDto>.Fail(400,errors));
            }
        }
    }
}
