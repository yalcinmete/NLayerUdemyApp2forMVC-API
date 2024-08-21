using Microsoft.AspNetCore.Diagnostics;
using NLayer.Core.DTOs;
using NLayer.Service.Exceptions;
using System.Text.Json;

namespace NLayer.API.Middlewares
{
    //Video38 Global Exception Handler.Program.cs dosyasını kirletmek istemediğimizden dolayı burada bir extension metot oluşturmak istedik. Extension metot yazabilmek için class'ında metotun da static olması gerekir. 
    //(this IApplicationBuilder app) diyerek IApplicationBuilder implemente eden classlar için extension metot yazabilmiş oluyoruz.
    public static class UseCustomExceptionHandler
    {
        public static void UseCustomException(this IApplicationBuilder app) 
        {
            //Bir exception fırlatıldığında UseExceptionHandler middleware'ı çalışır.Geriye bir model döner. Biz kendi modelimizi dönmek istiyoruz :
            app.UseExceptionHandler(config =>
            {
                //Run sonlandırıcı bir middleware'dir.Request buraya geldiği anda artık controllere vs gitmeyecek geriye dönecek.
                config.Run(async context =>
                {
                    context.Response.ContentType = "application/json";
                    var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();

                    //exceptionFeature.Error un içine switch ile giriyorum. Eğer hatanın tipi ClientSideException ise 400 dön ya da default ( _) 500 dön statusCode'a ata.
                    var statusCode = exceptionFeature.Error switch
                    {
                        ClientSideException => 400,
                        NotFoundException => 404, //Video39sonları.Idye ait nesne bulamazsa 400 değilde 404 dönmeli aslında.
                        _ => 500
                    };

                    //_=>500 Defaultta 500 hatasını clientlara dönmek doğru değil aslında. statusCode=500 ' e düşerse beklenmeyen bir hata oluştu şeklinde geri dönüş yapmak daha doğru olacaktır.

                    context.Response.StatusCode = statusCode;
                    var response = CustomResponseDto<NoContentDto>.Fail(statusCode, exceptionFeature.Error.Message);

                    //response bir tip. Bizim json dönmemiz gerekiyor. Bunu da belirtmek zorundayız.Controller tarafında controller returnlar nasıl objeleri dönüyor o halde ? Framework o tarafta bunu otomatik yapıyor.
                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                });
            });
        }
    }
}
