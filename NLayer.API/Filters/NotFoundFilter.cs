using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;

namespace NLayer.API.Filters
{
    //Video40 NotFoundFilter. Bir önceki global düzeyde bir işlemdi, servis katmanında data null ise exception fırlatıyorduk.Şimdi ise herhangi bir entity için data null olduğunda ek bir business yapmak gerekebilir(mesaj kuyruğuna bir mesaj atsın,kullanıcıya mail göndersin gibi).Bu gibi durumlarda ayrı bir filter yazmak çok daha başarılı olacaktır.
    //NotFoundFilter'ı dinamik yapalım.Sadece Product için olmasın.
    //public class NotFoundFilter<T> : IAsyncActionFilter where T : class

    //BaseEntity'de Id var.
    public class NotFoundFilter<T> : IAsyncActionFilter where T : BaseEntity //BaseEntity'de Id var.
    {
        private readonly IService<T> _service;

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
            //Data yok ise; 
            context.Result = new NotFoundObjectResult(CustomResponseDto<NoContentDto>.Fail(404, $"{typeof(T).Name}({id}) not found"));
        }
    }
}
