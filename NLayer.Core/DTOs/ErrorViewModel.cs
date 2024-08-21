using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.DTOs
{
    //MVC Video 53 NotFoundFilter. Kullanıcı hata ile karşılaştığında API tarafında Success,Fail 404 gibi hata mesajı dönüyorduk. Burada 404 durumu yok. Bu nedenle ErrorSayfası döndereceğiz.
    //API tarafında DTO iken, MVC tarafındaki ViewModel ismi verilir
    public class ErrorViewModel
    {
        public List<string> Errors { get; set; } = new List<string>(); //Listeyi bir yerlerde önce oluşturmalısın.Başka sayfalarda oluşturamıyorsan burada new List<string>() ile oluşturmuş ol.
    }
}
