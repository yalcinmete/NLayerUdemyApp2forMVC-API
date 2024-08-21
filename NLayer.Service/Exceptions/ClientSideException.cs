namespace NLayer.Service.Exceptions
{
    //Video38. UseCustomExceptionHandler'da kendi fırlattığımız hata mı yoksa , uygulamanın fırlattığı hata mı onu ayırt edebilmek için kendimize bir exception oluşturuyoruz.
    public class ClientSideException : Exception
    {
        public ClientSideException(string message) : base(message)
        {

        }
    }
}
