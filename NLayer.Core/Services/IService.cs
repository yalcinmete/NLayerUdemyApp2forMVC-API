using System.Linq.Expressions;

namespace NLayer.Core.Services
{
    public interface IService<T> where T : class
    {
        //IGenericRepository 'i kopyalayıp buraya yapıştırdık.Aslında duplicate işlem yapmıyoruz.İleride buradaki metotların dönüş tipleri değişecek.

        Task<T> GetByIdAsync(int id);

        //IQueryable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync(); //IGenericRepositoryden kopyaladığımıdan(IQueryable<T> GetAll()) farklı olsun diye bu metodu oluşturduk.

        //Task yerine IQueryable kullanmamızın nedeni , veritabanına yansımasını servisi çağıran kodda Tolist(),toListasync() çağırdıktan sonra veritabanına yansıması gerçekleşicek. 
        IQueryable<T> Where(Expression<Func<T, bool>> expression);
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);

        //Task AddAsync(T entity);
        Task<T> AddAsync(T entity);//Video27 Ekleme işleminden sonra T dönsün. Çünkü eklenen entitiy'nin id'si ihtiyaç olabilir

        //Task AddRangeAsync(IEnumerable<T> entities);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);//Video27 Ekleme işleminden sonra T dönsün. Çünkü eklenen entitileri'nin id'si ihtiyaç olabilir

        //Buradaki update,remove metotlarını service katmanında implemente ettiğimizde SaveChangesAsync kullanacağımız için metotları Task'a çevirdik.
        Task UpdateAsync(T entity);
        Task RemoveAsync(T entity);

        Task RemoveRangeAsync(IEnumerable<T> entities);
    }
}
