namespace NLayer.Core.UnitOfWorks
{
    //Repositoryler ayrı ayrı SaveChangeMetodlarını çağırdıklarında bir repositoryde savechangemetodu doğru çalışmadığında hata alabiliriz.SaveChange'i doğru şekilde yönetmek için bu interfaceı oluşturuyoruz.
    public interface IUnitOfWork
    {
        //SaveChangeAsync() kullanıldığı zaman.
        Task CommitAsync();

        void Commit();
    }
}
